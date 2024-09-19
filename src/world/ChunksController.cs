using VoxelGame.Logic;
using VoxelGame.Graphics;
using VoxelGame.Voxels;

using OpenTK.Mathematics;

namespace VoxelGame.World{
    public class ChunksController : GameObject{

        private Camera _camera;
        private VoxelStorage _voxelStorage;
        private uint _renderDistance = 5;
        private ChunkMeshBuilder _meshBuilder;

        private (int, int) chunkPos;

        public ChunksController(Camera Camera, VoxelStorage VoxelStorage){
            _camera = Camera;
            _voxelStorage = VoxelStorage;
            _meshBuilder = new ChunkMeshBuilder(_voxelStorage);
        }

        private void Start(){

        }

        public override void Update(){
            if (UpdateChunkPos()) LoadChunks();
        }

        // returns true if pos changed
        private bool UpdateChunkPos(){
            Vector3 camPos = _camera.GetPosition();
            (int, int) newChunkPos = ((int)camPos.X / 16, (int)camPos.Z / 16);
            if (newChunkPos != chunkPos) {
                chunkPos = newChunkPos;
                return true;
            }// } else {
            //     Console.WriteLine(newChunkPos + " = " + chunkPos);
            // }
            return false;
        }

        public void SetRenderDistance(uint newDistance) {
            _renderDistance = newDistance;
            LoadChunks();
        }

        private void LoadChunks() {
            Console.WriteLine("chunk changed");
            MeshRenderer chunkRenderer = new MeshRenderer(_camera);
            chunkRenderer.setPosition(new Vector3((float)chunkPos.Item1 * 16, 0.0f, (float)chunkPos.Item2 * 16));
            chunkRenderer.setMesh(_meshBuilder.BuildMeshOfChunkAt(chunkPos.Item1, chunkPos.Item2));
            chunkRenderer.setTexture(Texture.LoadFromFile("res/textures/container.png"));
            chunkRenderer.setShader(new Shader("res/shaders/shader.vert", "res/shaders/shader.frag"));
            _voxelStorage.GetOrCreateChunk(chunkPos.Item1, chunkPos.Item2).setMeshRenderer(chunkRenderer);
        }

    }
}