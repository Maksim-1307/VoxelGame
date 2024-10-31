using VoxelGame.Logic;
using VoxelGame.Graphics;
using VoxelGame.Voxels;
using System.Threading;
using OpenTK.Mathematics;
using System;
using System.Collections.Concurrent;

namespace VoxelGame.World{
    public class ChunksController : GameObject{

        private Camera _camera;
        private VoxelStorage _voxelStorage;
        private uint _renderDistance = 5;
        private ChunkRenderer _meshBuilder;
        private WorldRenderer _worldRenderer;

        private Thread renderingThread;

        private bool flag = true;

        private (int, int) chunkPos;

        public ChunksController(Camera Camera, VoxelStorage VoxelStorage){
            _camera = Camera;
            _voxelStorage = VoxelStorage;
            _meshBuilder = new ChunkRenderer(_voxelStorage);
            _worldRenderer = new WorldRenderer(_voxelStorage, _camera);
            LoadChunk(0,0);
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
            }
            return false;
        }

        public void SetRenderDistance(uint newDistance) {
            _renderDistance = newDistance;
        }

        private void LoadChunks() {
            if (!flag) return;
            LoadChunk(0, 0);
        }

        private void LoadChunk(int x, int z) {
            _voxelStorage.GetOrCreateChunk(x, z);
        }

        public bool IsObstableAt (Vector3 pos) {
            float x = pos.X;
            float y = pos.Y;
            float z = pos.Z;
            int vx = (int)Math.Floor(x);
            int vy = (int)Math.Floor(y);
            int vz = (int)Math.Floor(z);
            float ix = x - vx;
            float iy = y - vy;
            float iz = z - vz;
            Voxel vox = _voxelStorage.GetVoxel(vx, vy, vz);
            AABB[] AABBs = Block.GetAABBs(vox);
            foreach (AABB hitbox in AABBs){
                if (hitbox.contains(new Vector3(ix, iy, iz))) return true;
            }
            return false;
        }

        public Voxel? RayCast(Vector3 pos, Vector3 dir, float maxDist){
            Vector3 step = 0.1f * dir;
            Vector3 current = pos;
            while ((pos - current).Length < maxDist) {
                current += step;
                if (IsObstableAt(current)) {
                    int x = (int)MathF.Floor(current.X);
                    int y = (int)MathF.Floor(current.Y);
                    int z = (int)MathF.Floor(current.Z);
                    return _voxelStorage.GetVoxel(x, y, z);
                }
            }
            return null;
        }

    }
}