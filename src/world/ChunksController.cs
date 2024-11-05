using VoxelGame.Logic;
using VoxelGame.Graphics;
using VoxelGame.Voxels;
using VoxelGame.Lighting;
using System.Threading;
using OpenTK.Mathematics;
using System;
using System.Collections.Concurrent;

namespace VoxelGame.World{
    public class ChunksController : GameObject{

        private Camera _camera;
        private VoxelStorage _voxelStorage;
        private uint _renderDistance = 5;
        private LightSolver _lightSolver;
        private WorldRenderer _worldRenderer;

        private Thread renderingThread;

        private bool flag = true;

        private (int, int) chunkPos;

        public ChunksController(Camera Camera, VoxelStorage VoxelStorage, LightSolver lightSolver){
            _camera = Camera;
            _voxelStorage = VoxelStorage;
            _worldRenderer = new WorldRenderer(_voxelStorage, _camera);
            _lightSolver = lightSolver;
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
                    SetVoxel(x, y, z, new Voxel(0,0));
                    return _voxelStorage.GetVoxel(x, y, z);
                }
            }
            return null;
        }


        public void SetVoxel(int x, int y, int z, Voxel vox){
            _voxelStorage.SetVoxel(x, y, z, vox);
            (int x, int z) chunkPos = _voxelStorage.GetChunkPos(x, y, z);
            _lightSolver.Add(x, y, z, 8);
            _lightSolver.Solve();
            //_lightMap.UpdateChunkLights(chunkPos.x, chunkPos.z);
            //_lightMap.solveLightAt(x, y, z, 8);
            // _lightMap.solveLightAt(x+1, y, z, (byte)(_lightMap.GetLight(x+1, y, z).Value));
            // _lightMap.solveLightAt(x-1, y, z, (byte)(_lightMap.GetLight(x-1, y, z).Value));
            // _lightMap.solveLightAt(x, y+1, z, (byte)(_lightMap.GetLight(x, y+1, z).Value));
            // _lightMap.solveLightAt(x, y-1, z, _lightMap.GetLight(x, y-1, z).Value);
            // _lightMap.solveLightAt(x, y, z+1, _lightMap.GetLight(x, y, z-1).Value);
            // _lightMap.solveLightAt(x, y, z - 1, _lightMap.GetLight(x, y, z - 1).Value);

            _worldRenderer.UpdateChunk(chunkPos);

            _worldRenderer.UpdateChunk((chunkPos.x + 1, chunkPos.z));
            _worldRenderer.UpdateChunk((chunkPos.x - 1, chunkPos.z));
            _worldRenderer.UpdateChunk((chunkPos.x, chunkPos.z + 1));
            _worldRenderer.UpdateChunk((chunkPos.x, chunkPos.z - 1));

            // if (x % 16 == 0) {
            //     if (x > 0) {
            //         _worldRenderer.UpdateChunk((chunkPos.x-1, chunkPos.z));
            //     } else {
            //         _worldRenderer.UpdateChunk((chunkPos.x + 1, chunkPos.z));
            //     }
            // }
            // if (z % 16 == 0)
            // {
            //     if (z > 0)
            //     {
            //         _worldRenderer.UpdateChunk((chunkPos.x, chunkPos.z - 1));
            //     }
            //     else
            //     {
            //         _worldRenderer.UpdateChunk((chunkPos.x, chunkPos.z + 1));
            //     }
            // }
        }

    }
}