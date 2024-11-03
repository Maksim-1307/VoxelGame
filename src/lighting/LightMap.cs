using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using VoxelGame.Voxels;
using System.Runtime.CompilerServices;

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

        public void solveLightAt(int x, int y, int z, byte value){

            if (voxelStorage.GetVoxel(x, y, z).Id != 0) return;
            byte light = GetLight(x, y, z).Value;
            //Console.WriteLine("Lights updaed33");
            if (light >= value) return;

            SetLight(x, y, z, new Light(value));

            //Console.WriteLine("Lights updaed");

            solveLightAt(x-1, y, z, (byte)(value-1));
            solveLightAt(x+1, y, z, (byte)(value-1));
            solveLightAt(x, y-1, z, (byte)(value-1));
            solveLightAt(x, y+1, z, (byte)(value-1));
            solveLightAt(x, y, z-1, (byte)(value-1));
            solveLightAt(x, y, z+1, (byte)(value-1));
        }

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
                           //solveLightAt(chunkAbsPosX + x, y, chunkAbsPosZ + z, 8);
                            chunkLights.SetLight(x, y, z, new Light(0));
                        } else {
                            //solveLightAt(chunkAbsPosX + x, y, chunkAbsPosZ + z, 0);
                            chunkLights.SetLight(x, y, z, new Light(0));
                        }
                        // solvingQueue.Enqueue((chunkAbsPosX + x, y, chunkAbsPosZ + z));
                    }
                }
            }

            return chunkLights;
        }

        public void UpdateChunkLights(int chunkX, int chunkZ)
        {
            var key = (chunkX, chunkZ);
            if (lightMap.ContainsKey(key))
            {
                lightMap[key] = generateLights(chunkX, chunkZ);
            }
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