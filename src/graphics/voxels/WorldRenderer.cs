using System;
using System.IO;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Input;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Graphics.OpenGL4;
using StbImageSharp;
using OpenTK.Mathematics;
using VoxelGame.Voxels;
using System.Collections.Generic;
using VoxelGame.Lighting;

namespace VoxelGame.Graphics{
    public class WorldRenderer{

        public Dictionary<(int x, int z), Mesh> chunksMeshes = new Dictionary<(int, int), Mesh>();

        private VoxelStorage _voxelStorage;
        private LightMap _lightMap;
        private Shader _shader;
        private Texture _texture;
        private Texture _texture2;
        private Camera _camera;
        private ChunkRenderer _chunkRenderer;

        private Queue<(int chunkX, int chunkZ)> _renderQueue = new Queue<(int chunkX, int chunkZ)>();

        private Thread _renderThread;

        public static uint _renderDistance = 6;

        public WorldRenderer(VoxelStorage voxelStorage, LightMap lightMap, Camera camera){
            _voxelStorage = voxelStorage;
            _lightMap = lightMap;
            _camera = camera;
            _texture = Texture.LoadFromFile("res/textures/atlas.png");
            _texture2 = Texture.LoadFromFile("res/textures/atlas.png");
            _shader = new Shader("res/shaders/shader.vert", "res/shaders/shader.frag");
            _chunkRenderer = new ChunkRenderer(_voxelStorage, _lightMap);
            _renderThread  = new Thread(handleRenderQueue);
        }

        ~WorldRenderer(){
            _renderThread.Abort();
        }

        public void renderChunks((int x, int y) centerChunkPos){
            foreach ((int x, int z) chunkPos in _voxelStorage.chunks.Keys.ToArray())
            {
                int x = chunkPos.Item1 - centerChunkPos.Item1;
                int y = chunkPos.Item2 - centerChunkPos.Item2;
                if (x * x + y * y <= _renderDistance * _renderDistance)
                {
                    renderChunk(chunkPos.Item1, chunkPos.Item2);
                }
            }
        }

        public void renderChunk(int chunkX, int chunkZ){
            Mesh mesh = getMeshOfChunkAt(chunkX, chunkZ);

            if (mesh == null) return;

            _texture.Use(TextureUnit.Texture0);
            _texture2.Use(TextureUnit.Texture1);
            _shader.Use();

            var model = Matrix4.Identity * Matrix4.CreateTranslation(chunkX*0.0f, 0.0f, chunkZ*0.0f);

            _shader.SetMatrix4("model", model);
            _shader.SetMatrix4("view", _camera.GetViewMatrix());
            _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());

            mesh.draw();
        }

        public Mesh getMeshOfChunkAt(int chunkX, int chunkZ){

            var key = (chunkX, chunkZ);

            toggleRenderQueue();

            if (!chunksMeshes.ContainsKey(key))
            {
                if (!_renderQueue.Contains(key)) _renderQueue.Enqueue(key);
                return null;
            } else {
                return chunksMeshes[key];
            }
        }

        void toggleRenderQueue(){
            if (_renderQueue.Count > 0 && (_renderThread == null || !_renderThread.IsAlive)){
                _renderThread = new Thread(handleRenderQueue);
                _renderThread.Start();
            }
        }
        void handleRenderQueue(){

            while (_renderQueue.Count > 0){
                (int x, int z) chunkPos = _renderQueue.Dequeue();
                chunksMeshes[chunkPos] = _chunkRenderer.BuildMeshOfChunkAt(chunkPos.Item1, chunkPos.Item2);                
            }
            return;
        }

        public void UpdateChunk((int x, int z) chunkPos) {
            _renderQueue.Enqueue(chunkPos);
        }
    }
}