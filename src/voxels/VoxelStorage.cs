using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using VoxelGame.Lighting;

namespace VoxelGame.Voxels{

    public class VoxelStorage
    {
        public ConcurrentDictionary<(int x, int z), Chunk> chunks = new ConcurrentDictionary<(int, int), Chunk>();
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

        public Chunk? GetChunk(int chunkX, int chunkZ)
        {
            var key = (chunkX, chunkZ);
            if (!chunks.ContainsKey(key))
            {
                return null;
            }
            return chunks[key];
        }

        public Voxel GetVoxel(int x, int y, int z)
        {
            int chunkX = x / 16;
            int chunkZ = z / 16;

            int blockX = x % 16;
            int blockZ = z % 16;

            // transformations for negative directions
            if (chunkX == 0 && x < 0){
                chunkX = -1;
                blockX += 16;
            } else if (chunkX < 0) {
                blockX += 16;
                chunkX -= 1;
                if (blockX == 16) {
                    blockX = 0;
                    chunkX += 1;
                }
            }
            if (chunkZ == 0 && z < 0){
                chunkZ = -1;
                blockZ += 16;
            } else if (chunkZ < 0){
                blockZ += 16;
                chunkZ -= 1;
                if (blockZ == 16) {
                    blockZ = 0;
                    chunkZ += 1;
                }
            }

            Chunk chunk = GetOrCreateChunk(chunkX, chunkZ);
            return chunk.GetVoxel(blockX, y, blockZ);
        }

        public Light GetLight(int x, int y, int z)
        {
            (int X, int Z) chunkPos = GetChunkPos(x, y, z);

            if (!chunks.ContainsKey(chunkPos)) return new Light(0);
            Chunk chunk = chunks[chunkPos];

            (int X, int Y, int Z) blockPos = (x - chunkPos.X * 16, y, z - chunkPos.Z * 16);
            return chunk.lightMap.GetLight(blockPos.X, blockPos.Y, blockPos.Z);
        }

        public (int x, int z) GetChunkPos(int x, int y, int z)
        {
            int chunkX = (int)MathF.Floor((float)x / 16);
            int chunkZ = (int)MathF.Floor((float)z / 16);

            return (chunkX, chunkZ);
        }

        public void SetVoxel(int x, int y, int z, Voxel voxel)
        {
            int chunkX = x / 16;
            int chunkZ = z / 16;

            int blockX = x % 16;
            int blockZ = z % 16;

            // transformations for negative directions
            if (chunkX == 0 && x < 0)
            {
                chunkX = -1;
                blockX += 16;
            }
            else if (chunkX < 0)
            {
                blockX += 16;
                chunkX -= 1;
                if (blockX == 16)
                {
                    blockX = 0;
                    chunkX += 1;
                }
            }
            if (chunkZ == 0 && z < 0)
            {
                chunkZ = -1;
                blockZ += 16;
            }
            else if (chunkZ < 0)
            {
                blockZ += 16;
                chunkZ -= 1;
                if (blockZ == 16)
                {
                    blockZ = 0;
                    chunkZ += 1;
                }
            }

            Chunk chunk = GetOrCreateChunk(chunkX, chunkZ);
            chunk.SetVoxel(blockX, y, blockZ, voxel);
        }

        public Chunk? GetChunkByVoxel(int x, int y, int z){
            (int cx, int cz) pos = GetChunkPos(x, y, z);
            if (chunks.ContainsKey(pos)) {
                return chunks[pos];
            }
            return null;
        }
    }
}