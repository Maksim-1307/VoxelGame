using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using VoxelGame.Voxels;

namespace VoxelGame.Lighting
{

    public class LightMap
    {
        public ConcurrentDictionary<(int x, int z), ChunkLightMap> lightMap = new ConcurrentDictionary<(int, int), ChunkLightMap>();
        private VoxelStorage voxelStorage;

        public LightMap(VoxelStorage voxelStorage)
        {
            this.voxelStorage = voxelStorage;
        }

        //testing 
        private ChunkLightMap generateLights(int chunkX, int chunkZ){
            ChunkLightMap chunkLights = new ChunkLightMap();

            int chunkAbsPosX = chunkX * 16;
            int chunkAbsPosZ = chunkZ * 16;


            for (int x = 0; x < 16; x++)
            {
                for (int z = 0; z < 16; z++)
                {
                    bool flag = true;
                    for (int y = 255; y >= 0; y--)
                    {
                        if (voxelStorage.GetVoxel(chunkAbsPosX + x, y, chunkAbsPosZ + z).Id != 0) flag = false;
                        if (flag) {
                            chunkLights.SetLight(x, y, z, new Light(8));
                        } else {
                            chunkLights.SetLight(x, y, z, new Light(0));
                        }
                    }
                }
            }

            return chunkLights;
        }

        public ChunkLightMap GetOrCreateChunkLightMap(int chunkX, int chunkZ)
        {
            var key = (chunkX, chunkZ);
            if (!lightMap.ContainsKey(key))
            {
                lightMap[key] = generateLights(chunkX, chunkZ);
            }
            return lightMap[key];
        }

        public Light GetLight(int x, int y, int z)
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

            ChunkLightMap chunkMap = GetOrCreateChunkLightMap(chunkX, chunkZ);
            return chunkMap.GetLight(blockX, y, blockZ);
        }

        public (int x, int z) GetChunkPos(int x, int y, int z)
        {
            int chunkX = (int)MathF.Floor((float)x / 16);
            int chunkZ = (int)MathF.Floor((float)z / 16);

            return (chunkX, chunkZ);
        }

        public void SetLight(int x, int y, int z, Light light)
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

            ChunkLightMap chunkMap = GetOrCreateChunkLightMap(chunkX, chunkZ);
            chunkMap.SetLight(blockX, y, blockZ, light);
        }
    }
}