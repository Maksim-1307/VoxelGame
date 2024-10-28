global using static FastNoiseLite;

namespace VoxelGame.Voxels{
    public class Generator {

        FastNoiseLite noise;

        public Generator() {
            noise = new FastNoiseLite(1000);
            noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
            noise.SetFrequency(0.01f);
            noise.SetFractalLacunarity(2f);
            noise.SetFractalGain(0.5f);
            noise.SetFractalType(FastNoiseLite.FractalType.FBm);
        }

        public Chunk GenerateChunkAt(int chunkX, int chunkZ){

            Chunk chunk = new Chunk();

            int chunkAbsPosX = chunkX * 16;
            int chunkAbsPosZ = chunkZ * 16;


            for (int x = 0; x < 16; x++){
                for (int z = 0; z < 16; z++){
                    int height = (int)(32 * ( 1.0f + noise.GetNoise(chunkAbsPosX + x, chunkAbsPosZ + z)));
                    for (int y = 0; y < 256; y++){
                        //Console.WriteLine(height);
                        if (y < height) {
                            if (y < 30){
                                chunk.SetVoxel(x, y, z, new Voxel(1, 0));
                            } else {
                                chunk.SetVoxel(x, y, z, new Voxel(2, 0));
                            }
                        } else {
                            chunk.SetVoxel(x, y, z, new Voxel(0,0));
                        }
                    }
                }
            }

            return chunk;
        }
    }
}