using System;
using System.Collections.Generic;

namespace VoxelGame.Voxels{

    public class VoxelStorage
    {
        public Dictionary<(int x, int z), Chunk> chunks = new Dictionary<(int, int), Chunk>();
        public Generator Generator;

        public VoxelStorage (Generator generator) {
            this.Generator = generator;
        }

        public Chunk GetOrCreateChunk(int chunkX, int chunkZ)
        {
            var key = (chunkX, chunkZ);
            if (!chunks.ContainsKey(key))
            {
                chunks[key] = Generator.GenerateChunkAt(chunkX, chunkZ);
            }
            return chunks[key];
        }

        public Voxel GetVoxel(int x, int y, int z)
        {
            int chunkX = x / 16;
            int chunkZ = z / 16;

            int blockX = x % 16;
            int blockZ = z % 16;

            //if (blockX < 0) blockX += 16; //chunkX -= 1;
            //if (blockZ < 0) blockZ += 16; //chunkZ -= 1;
            if (chunkX < 0) {
                blockX += 16;
                chunkX -= 1;
                if (blockX == 16) {
                    blockX = 0;
                    chunkZ += 1;
                }
            }
            if (chunkZ < 0){
                blockZ += 16;
                chunkZ -= 1;
                if (blockZ == 16) {
                    blockZ = 0;
                    chunkX += 1;
                }
            }

            Chunk chunk = GetOrCreateChunk(chunkX, chunkZ);
            return chunk.GetVoxel(blockX, y, blockZ);
        }

        public void SetVoxel(int x, int y, int z, Voxel voxel)
        {
            int chunkX = x / 16;
            int chunkZ = z / 16;

            int blockX = x % 16;
            int blockZ = z % 16;

            Chunk chunk = GetOrCreateChunk(chunkX, chunkZ);
            chunk.SetVoxel(blockX, y, blockZ, voxel);
        }
    }
}