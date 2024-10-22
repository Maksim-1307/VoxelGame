using VoxelGame.Logic;
using VoxelGame.Graphics;
using VoxelGame.Voxels;

using OpenTK.Mathematics;

namespace VoxelGame.World{
    public class ChunksController : GameObject{

        private Camera _camera;
        private VoxelStorage _voxelStorage;
        private uint _renderDistance = 5;
        private ChunkRenderer _meshBuilder;
        private WorldRenderer _worldRenderer;

        private bool flag = true;

        private (int, int) chunkPos;

        public ChunksController(Camera Camera, VoxelStorage VoxelStorage){
            _camera = Camera;
            _voxelStorage = VoxelStorage;
            _meshBuilder = new ChunkRenderer(_voxelStorage);
            _worldRenderer = new WorldRenderer(_voxelStorage, _camera);
        }

        public override void Start(){
            LoadChunk(0, 0);
        }

        public override void Update(){
            if (UpdateChunkPos()) LoadChunk(chunkPos.Item1, chunkPos.Item2);
            _worldRenderer.renderChunks(chunkPos);
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
            //LoadChunks();
        }

        private void LoadChunks() {
            if (!flag) return;
            Console.WriteLine("chunk changed");
            LoadChunk(0, 0);
        }

        private void LoadChunk(int x, int z) {
            _voxelStorage.GetOrCreateChunk(x, z);
        }

    }
}