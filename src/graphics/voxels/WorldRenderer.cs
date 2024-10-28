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

namespace VoxelGame.Graphics{
    public class WorldRenderer{

        public Dictionary<(int x, int z), Mesh> chunksMeshes = new Dictionary<(int, int), Mesh>();

        private VoxelStorage _voxelStorage;
        private Shader _shader;
        private Texture _texture;
        private Texture _texture2;
        private Camera _camera;
        private ChunkRenderer _chunkRenderer;

        private Queue<(int chunkX, int chunkZ)> _renderQueue = new Queue<(int chunkX, int chunkZ)>();

        private Thread _renderThread;

        public static uint _renderDistance = 6;

        public WorldRenderer(VoxelStorage voxelStorage, Camera camera){
            _voxelStorage = voxelStorage;
            _camera = camera;
            _texture = Texture.LoadFromFile("res/textures/atlas.png");
            _texture2 = Texture.LoadFromFile("res/textures/atlas.png");
            _shader = new Shader("res/shaders/shader.vert", "res/shaders/shader.frag");
            _chunkRenderer = new ChunkRenderer(_voxelStorage);
            _renderThread  = new Thread(handleRenderQueue);
        }

        ~WorldRenderer(){
            _renderThread.Abort();
        }

        public void renderChunks((int x, int y) centerChunkPos){
            //Console.WriteLine("Count of chunks: " + _voxelStorage.chunks.Count);
            // (int x, int z)[] chunks;
            // lock ()
            // {
            //     chunks = _voxelStorage.chunks.Keys.ToArray();
            // }
            // _dict.Keys.ElementAt(5)
            // for (int i = 0; i < _voxelStorage.chunks.Count; i++){
            //     (int x, int z) chunkPos = _voxelStorage.chunks.Keys.ElementAt(i);
            // // }
            //lock(chunks){
            foreach ((int x, int z) chunkPos in _voxelStorage.chunks.Keys.ToArray())
            {
                int x = chunkPos.Item1 - centerChunkPos.Item1;
                int y = chunkPos.Item2 - centerChunkPos.Item2;
                if (x * x + y * y <= _renderDistance * _renderDistance)
                {

                    renderChunk(chunkPos.Item1, chunkPos.Item2);
                    // if (_renderThread == null) {
                    //     _renderThread = new Thread(() => ;
                    //     _renderThread.Start();
                    // }
                }
            }
            //}
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
                // add render queue here
                // render queue add (chunkX, chunkZ)
                //Console.WriteLine(chunksMeshes.Count + ", " + _renderQueue.Count);
                if (!_renderQueue.Contains(key)) _renderQueue.Enqueue(key);
                return null;
                // return (chunk mesh will be added in chunksMeshes in a thread)
                //chunksMeshes[key] = _chunkRenderer.BuildMeshOfChunkAt(chunkX, chunkZ);
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
                //Console.WriteLine("just after");
                //Mesh mesh = new Mesh([],[]);
                chunksMeshes[chunkPos] = _chunkRenderer.BuildMeshOfChunkAt(chunkPos.Item1, chunkPos.Item2);
                //renderChunk(chunkPos.x, chunkPos.z);
                
            }
            
            return;
            // foreach ((int x, int z) chunkPos in _renderQueue){
            //     renderChunk(chunkPos.x, chunkPos.z);
            // }
        }

        /*
        function toggleRenderQueueHandling(){
            if (queue is not empty and renderThread is not running){
                run render thread (render function)
            }
        }
        function renderFunction(){
            while (queue is not empty){
                chunksMeshes[queueElem] = _chunkRenderer.BuildMeshOfChunkAt(queueElem);
            }
        }
        */


    }
}