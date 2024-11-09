using System.Threading.Tasks.Sources;
using VoxelGame.Lighting;
using VoxelGame.Voxels;

namespace VoxelGame.Lighting{
    public class Lighting{
        private LightSolver solver;
        private VoxelStorage voxelStorage;
        public Lighting(VoxelStorage voxelStorage){
            solver = new LightSolver(0);
            this.voxelStorage = voxelStorage;
        }

        public void PreBuildSkyLight(Chunk chunk){

            int highestPoint = 0;
            for (int z = 0; z < 16; z++)
            {
                for (int x = 0; x < 16; x++)
                {
                    for (int y = 256 - 1; y >= 0; y--)
                    {
                        Voxel vox = chunk.GetVoxel(x, y, z);
                        Block block = Block.GetBlockByVoxelId(vox.Id);
                        if (vox.Id != 0)
                        {
                            if (highestPoint < y) highestPoint = y;
                            break;
                        }
                        chunk.lightMap.SetLight(x, y, z, new Light(15));
                    }
                }
            }
            if (highestPoint < 256 - 1) highestPoint++;
            chunk.lightMap.highestPoint = highestPoint;
        }

        public void BuildSkyLight(int cx, int cz)
        {
            Chunk chunk = voxelStorage.GetChunk(cx, cz);
            for (int z = 0; z < 16; z++)
            {
                for (int x = 0; x < 16; x++)
                {
                    int gx = x + cx * 16;
                    int gz = z + cz * 16;
                    for (int y = chunk.lightMap.highestPoint; y >= 0; y--)
                    {
                        while (y > 0 && chunk.GetVoxel(x, y, z).Id != 0)
                        {
                            y--;
                        }
                        if (chunk.lightMap.GetLight(x, y, z).Value != 15)
                        {
                            solver.Add(gx, y + 1, gz);
                            for (; y >= 0; y--)
                            {
                                solver.Add(gx + 1, y, gz);
                                solver.Add(gx - 1, y, gz);
                                solver.Add(gx, y, gz + 1);
                                solver.Add(gx, y, gz - 1);
                            }
                        }
                    }
                }
            }
            solver.Solve();
        }


        public void OnChunkLoaded(int cx, int cz, bool expand)
        {
            Chunk chunk = voxelStorage.GetChunk(cx, cz);

            for (int y = 0; y < 256; y++)
            {
                for (int z = 0; z < 16; z++)
                {
                    for (int x = 0; x < 16; x++)
                    {
                        Voxel vox = chunk.GetVoxel(x, y, z);
                        Block block = Block.GetBlockByVoxelId(vox.Id);
                        int gx = x + cx * 16;
                        int gz = z + cz * 16;
                        // if (block->rt.emissive)
                        // {
                        //     solverR->add(gx, y, gz, block->emission[0]);
                        //     solverG->add(gx, y, gz, block->emission[1]);
                        //     solverB->add(gx, y, gz, block->emission[2]);
                        // }
                    }
                }
            }

            if (expand)
            {
                for (int x = 0; x < 16; x += 16 - 1)
                {
                    for (int y = 0; y < 256; y++)
                    {
                        for (int z = 0; z < 16; z++)
                        {
                            Light light = chunk.lightMap.GetLight(x, y, z);
                            if (light.Value > 1)
                            {
                                solver.Add(x, y, z);
                            }
                        }
                    }
                }
                for (int z = 0; z < 16; z += 16 - 1)
                {
                    for (int y = 0; y < 256; y++)
                    {
                        for (int x = 0; x < 16; x++)
                        {
                            Light light = chunk.lightMap.GetLight(x, y, z);
                            if (light.Value > 1)
                            {
                                solver.Add(x, y, z);
                            }
                        }
                    }
                }
            }
            solver.Solve();
        }

    


        // public void Clear(){
        //     for (int index = 0; index < voxelStorage.chunks.Count; index++)
        //     {
        //         Chunk chunk = voxelStorage.chunks[index];
        //         if (chunk == nullptr)
        //             continue;
        //         Lightmap & lightmap = chunk->lightmap;
        //         for (int i = 0; i < CHUNK_VOL; i++)
        //         {
        //             lightmap.map[i] = 0;
        //         }
        //     }
        // }


        public void OnBlockSet(int x, int y, int z, byte voxelId){
            Block block = Block.GetBlockByVoxelId(voxelId);
            
            // solverR->remove(x, y, z);
            // solverG->remove(x, y, z);
            // solverB->remove(x, y, z);

            if (voxelId == 0)
            {
                solver.Solve();
                // solverG->solve();
                // solverB->solve();
                if (voxelStorage.GetLight(x, y + 1, z).Value == 15)
                {
                    for (int i = y; i >= 0; i--)
                    {
                        Voxel vox = voxelStorage.GetVoxel(x, i, z);
                        if (vox.Id == 0)
                            break;
                        solver.Add(x, i, z, 15);
                    }
                }
                solver.Add(x, y + 1, z);
                solver.Add(x, y - 1, z);
                solver.Add(x + 1, y, z);
                solver.Add(x - 1, y, z);
                solver.Add(x, y, z + 1);
                solver.Add(x, y, z - 1);
                solver.Solve();
            }
            else
            {
                // if (voxelId == 0)
                // {
                //     //solverS->remove(x, y, z);
                //     for (int i = y - 1; i >= 0; i--)
                //     {
                //         solverS->remove(x, i, z);
                //         if (i == 0 || chunks->get(x, i - 1, z)->id != 0)
                //         {
                //             break;
                //         }
                //     }
                //     solverS->solve();
                // }
                // solverR->solve();
                // solverG->solve();
                // solverB->solve();

                // if (block->emission[0] || block->emission[1] || block->emission[2])
                // {
                //     solverR->add(x, y, z, block->emission[0]);
                //     solverG->add(x, y, z, block->emission[1]);
                //     solverB->add(x, y, z, block->emission[2]);
                //     solverR->solve();
                //     solverG->solve();
                //     solverB->solve();
                // }
            }
        }
    }
}
