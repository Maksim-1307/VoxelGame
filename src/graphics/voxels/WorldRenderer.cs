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

namespace VoxelGame.Graphics{
    public class WorldRenderer{

        public Dictionary<(int x, int z), Mesh> chunksMeshes = new Dictionary<(int, int), Mesh>();

        private VoxelStorage _voxelStorage;
        private Shader _shader;
        private Texture _texture;
        private Texture _texture2;
        private Camera _camera;
        private ChunkRenderer _chunkRenderer;

        public static uint _renderDistance = 6;

        public WorldRenderer(VoxelStorage voxelStorage, Camera camera){
            _voxelStorage = voxelStorage;
            _camera = camera;
            _texture = Texture.LoadFromFile("res/textures/texture.png");
            _texture2 = Texture.LoadFromFile("res/textures/texture.png");
            _shader = new Shader("res/shaders/shader.vert", "res/shaders/shader.frag");
            _chunkRenderer = new ChunkRenderer(_voxelStorage);
        }

        public void renderChunks((int x, int y) centerChunkPos){
            foreach((int x, int z) chunkPos in _voxelStorage.chunks.Keys.ToArray()) {
                int x = chunkPos.Item1 - centerChunkPos.Item1;
                int y = chunkPos.Item2 - centerChunkPos.Item2;
                if (x*x + y*y <= _renderDistance * _renderDistance){
                    renderChunk(chunkPos.Item1, chunkPos.Item2);
                }
            }
        }

        public void renderChunk(int chunkX, int chunkZ){
            Mesh mesh = getMeshOfChunkAt(chunkX, chunkZ);

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
            if (!chunksMeshes.ContainsKey(key))
            {
            Console.WriteLine(chunkX + ", " + chunkZ);

                chunksMeshes[key] = _chunkRenderer.BuildMeshOfChunkAt(chunkX, chunkZ);
            }
            return chunksMeshes[key];

        }


    }
}