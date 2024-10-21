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