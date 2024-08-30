namespace VoxelGame.Voxels{
    public class Generator {
        public Generator() {}

        public Chunk GenerateChunkAt(int chunkX, int chunkZ){

            Chunk chunk = new Chunk();

            for (int x = 0; x < 16; x++){
                for (int z = 0; z < 16; z++){
                    for (int y = 0; y < 256; y++){
                        if (y < 15) {
                            chunk.SetVoxel(x, y, z, new Voxel(1,0));
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