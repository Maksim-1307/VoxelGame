
using OpenTK.Graphics.OpenGL;
using VoxelGame.Voxels;

namespace VoxelGame.Lighting{

    public struct LightEntry {
        public int x, y, z;
        public byte emission;
        public LightEntry(int x, int y, int z, byte emission){
            this.x = x;
            this.y = y;
            this.z = z;
            this.emission = emission;
        }
    }

    public class LightSolver {

        private VoxelStorage _voxelStorage;
        private int _channel;
        private Queue<LightEntry> AddQueue;
        public LightSolver(VoxelStorage voxelStorage, int channel){
            _voxelStorage = voxelStorage;
            _channel = channel;
            AddQueue = new Queue<LightEntry>();
        }

        public void Add(int x, int y, int z, byte emission)
        {
            if (emission <= 1) return;

            AddQueue.Enqueue(new LightEntry(x, y, z, emission));

            Chunk chunk = _voxelStorage.GetChunkByVoxel(x, y, z);
            if (chunk == null) return;

            //chunk->flags.modified = true;
        }

        public void Add(int x, int y, int z)
        {
            Add(x, y, z, _voxelStorage.GetLight(x, y, z).Value);
        }


        public void Solve()
        {
            int [] coords = [
                0, 0, 1,
                0, 0,-1,
                0, 1, 0,
                0,-1, 0,
                1, 0, 0,
                -1, 0, 0
            ];

            // while (!remqueue.empty())
            // {
            //     const lightentry entry = remqueue.front();
            //     remqueue.pop();

            //     for (int i = 0; i < 6; i++)
            //     {
            //         int imul3 = i * 3;
            //         int x = entry.x + coords[imul3];
            //         int y = entry.y + coords[imul3 + 1];
            //         int z = entry.z + coords[imul3 + 2];

            //         Chunk* chunk = chunks->getChunkByVoxel(x, y, z);
            //         if (chunk)
            //         {
            //             int lx = x - chunk->x * CHUNK_W;
            //             int lz = z - chunk->z * CHUNK_D;
            //             chunk->flags.modified = true;

            //             ubyte light = chunk->lightmap.get(lx, y, lz, channel);
            //             if (light != 0 && light == entry.light - 1)
            //             {
            //                 remqueue.push(lightentry { x, y, z, light});
            //                 chunk->lightmap.set(lx, y, lz, channel, 0);
            //             }

            //                 else if (light >= entry.light)
            //             {
            //                 addqueue.push(lightentry { x, y, z, light});
            //             }
            //         }
            //     }
            // }

            //const Block* const* blockDefs = contentIds->getBlockDefs();
            while (AddQueue.Count > 0) {
                LightEntry entry = AddQueue.Dequeue();

                for (int i = 0; i < 6; i++)
                {
                    int imul3 = i * 3;
                    int x = entry.x + coords[imul3];
                    int y = entry.y + coords[imul3 + 1];
                    int z = entry.z + coords[imul3 + 2];

                    Chunk chunk = _voxelStorage.GetChunkByVoxel(x, y, z);
                    (int X, int Z) chunkPos = _voxelStorage.GetChunkPos(x, y, z);
                    if (chunk == null) i++;

                    int lx = x - chunkPos.X * 16;
                    int lz = z - chunkPos.Z * 16;
                    //chunk->flags.modified = true;

                    byte light = chunk.lightMap.GetLight(lx, y, lz).Value;
                    Voxel voxel = chunk.GetVoxel(lx, y, lz);
                    Block block = Block.GetBlockByVoxelId(voxel.Id);

                    if (voxel.Id == 0 && light + 2 <= entry.emission)
                    {
                        chunk.lightMap.SetLight(x - chunkPos.X * 16, y, z - chunkPos.Z * 16, new Light((byte)(entry.emission - 1)));
                        AddQueue.Enqueue(new LightEntry (x, y, z, (byte)(entry.emission - 1)));
                    }
                }
            }
        }
    }
}




// void LightSolver::remove(int x, int y, int z) {
//     Chunk* chunk = chunks->getChunkByVoxel(x, y, z);
//     if (chunk == nullptr)
//         return;

//     ubyte light = chunk->lightmap.get(x - chunk->x * CHUNK_W, y, z - chunk->z * CHUNK_D, channel);
//     if (light == 0)
//     {
//         return;
//     }
//     remqueue.push(lightentry { x, y, z, light});
//     chunk->lightmap.set(x - chunk->x * CHUNK_W, y, z - chunk->z * CHUNK_D, channel, 0);
// }
