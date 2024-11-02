using VoxelGame.Voxels;
using OpenTK.Mathematics;
using VoxelGame.Lighting;

namespace VoxelGame.Graphics{
    public class ChunkRenderer{

        private VoxelStorage VoxelStorage;
        private LightMap LightMap;

        private List<float> vertices = new List<float>(1024);
        private List<uint> indices = new List<uint>(256);
        private uint indexOffset = 0;
        const uint ATLAS_SIZE = 8;

        private uint verticesIndex = 0;
        private uint indicesIndex = 0;

        private const uint VERTEX_SIZE = 8; // x y z tx ty r g b

        private int _x;
        private int _y;
        private int _z;

        public ChunkRenderer (VoxelStorage VoxelStorage, LightMap LightMap){
            this.VoxelStorage = VoxelStorage;
            this.LightMap = LightMap;
        }

        public Mesh BuildMeshOfChunkAt(int chunkX, int chunkZ){

            vertices = new List<float>(1024);
            indices = new List<uint>(256);
            indexOffset = 0;
            verticesIndex = 0;
            indicesIndex = 0;

            int blockX = chunkX * 16;
            int blockZ = chunkZ * 16;

            for (_x = blockX; _x < blockX + 16; _x++){
                for (_z = blockZ; _z < blockZ + 16; _z++){
                    for (_y = 0; _y < 256; _y++){
                        if (VoxelStorage.GetVoxel(_x, _y, _z).Id != 0) CubeModel(_x, _y, _z);
                    }
                }
            }

            return new Mesh(vertices.ToArray(), indices.ToArray());
        }

        private bool[] OpenedAround(int x, int y, int z) {
            bool [] opened = new bool[6];
            opened[0] = Block.GetBlockByVoxelId(VoxelStorage.GetVoxel(x + 1, y, z).Id).OpenedFaces[adjacent(0)];
            opened[1] = Block.GetBlockByVoxelId(VoxelStorage.GetVoxel(x - 1, y, z).Id).OpenedFaces[adjacent(1)];
            opened[2] = Block.GetBlockByVoxelId(VoxelStorage.GetVoxel(x, y + 1, z).Id).OpenedFaces[adjacent(2)];
            opened[3] = Block.GetBlockByVoxelId(VoxelStorage.GetVoxel(x, y - 1, z).Id).OpenedFaces[adjacent(3)];
            opened[4] = Block.GetBlockByVoxelId(VoxelStorage.GetVoxel(x, y, z + 1).Id).OpenedFaces[adjacent(4)];
            opened[5] = Block.GetBlockByVoxelId(VoxelStorage.GetVoxel(x, y, z - 1).Id).OpenedFaces[adjacent(5)];

            // opened[0] = VoxelStorage.GetVoxel(x+1, y, z).Id == 0;
            // opened[1] = VoxelStorage.GetVoxel(x-1, y, z).Id == 0;
            // opened[2] = VoxelStorage.GetVoxel(x, y+1, z).Id == 0;
            // opened[3] = VoxelStorage.GetVoxel(x, y-1, z).Id == 0;
            // opened[4] = VoxelStorage.GetVoxel(x, y, z+1).Id == 0;
            // opened[5] = VoxelStorage.GetVoxel(x, y, z-1).Id == 0;
            return opened;
        }

        // returns number of face in same axis but reverse direction
        // for example: from X+ face to X- face, from Z+ face to Z- face
        private byte adjacent(byte face) 
        {
            if (face % 2 == 0) return (byte)(face + 1);
            return (byte)(face - 1);
        }

        private void vertex (float x, float y, float z, float uvx, float uvy) {
            (float r, float g, float b) color;
            float lihgt = 1.0f - (float)(this.LightMap.GetLight((int)(_x + x), (int)(_y + y), (int)(_z + z)).Value + 6 + this.LightMap.GetLight(_x, _y, _z).Value + 3) / 22.0f;
            color = (lihgt, lihgt, lihgt);
            // if ((x + z + y) % 2 == 0) {
            //     color = (1.0f, 0.0f, 0.0f);
            // } else {
            //     color = (0.0f, 0.0f, 1.0f);
            // }
            vertices.AddRange([ _x + x,  _y + y, _z + z,  uvx,  uvy, color.r, color.g, color.b]);
        }

        private void index(uint a, uint b, uint c, uint d, uint e, uint f) {
            indices.Add(indexOffset + a);
            indices.Add(indexOffset + b);
            indices.Add(indexOffset + c);
            indices.Add(indexOffset + d);
            indices.Add(indexOffset + e);
            indices.Add(indexOffset + f);
            indexOffset += 4;
        }

        /*

        faceVertices order:

        (3) ------- (2)
         |           |
         |           |
         |           |
        (0) ------- (1)
        
        */
        // private void SimpleFace (Vector3 [] faceVertices, Vector2 uvCoord1, Vector2 uvCoord2) {
        //     vertex(new List<float>{faceVertices[0].X, faceVertices[0].Y, faceVertices[0].Z, uvCoord1.X, uvCoord1.Y});
        //     vertex(new List<float>{faceVertices[1].X, faceVertices[1].Y, faceVertices[1].Z, uvCoord2.X, uvCoord1.Y});
        //     vertex(new List<float>{faceVertices[2].X, faceVertices[2].Y, faceVertices[2].Z, uvCoord2.X, uvCoord2.Y});
        //     vertex(new List<float>{faceVertices[3].X, faceVertices[3].Y, faceVertices[3].Z, uvCoord1.X, uvCoord2.Y});
        //     index(0, 1, 3, 1, 2, 3);
        // }

        private void CubeModel(int x, int y, int z){
            bool [] OpenedFaces = OpenedAround(x, y, z);
            Block block = Block.GetBlockByVoxelId(VoxelStorage.GetVoxel(x,y,z).Id);
            
            for (int face = 0; face < 6; face++){
                if (OpenedFaces[face]){
                (uint x, uint y) blockFaceUV = block.GetUV((byte)face);
                (float x, float y) uv = ((float)blockFaceUV.x / ATLAS_SIZE, (float)blockFaceUV.y / ATLAS_SIZE);
                switch (face) {
                    // X+
                    case 0:
                        vertex(1.0f, 0.0f, 0.0f, uv.x + 1.0f / ATLAS_SIZE, uv.y);
                        vertex(1.0f, 0.0f, 1.0f, uv.x, uv.y);
                        vertex(1.0f, 1.0f, 1.0f, uv.x, uv.y + 1.0f / ATLAS_SIZE);
                        vertex(1.0f, 1.0f, 0.0f, uv.x + 1.0f / ATLAS_SIZE, uv.y + 1.0f / ATLAS_SIZE);
                        break;
                    // X-
                    case 1:
                        vertex(0.0f, 0.0f, 0.0f, uv.x + 1.0f / ATLAS_SIZE, uv.y);
                        vertex(0.0f, 0.0f, 1.0f, uv.x, uv.y);
                        vertex(0.0f, 1.0f, 1.0f, uv.x, uv.y + 1.0f / ATLAS_SIZE);
                        vertex(0.0f, 1.0f, 0.0f, uv.x + 1.0f / ATLAS_SIZE, uv.y + 1.0f / ATLAS_SIZE);
                        break;
                    // Y+
                    case 2:
                        vertex(0.0f, 1.0f, 0.0f, uv.x, uv.y);
                        vertex(0.0f, 1.0f, 1.0f, uv.x, uv.y + 1.0f / ATLAS_SIZE);
                        vertex(1.0f, 1.0f, 1.0f, uv.x + 1.0f / ATLAS_SIZE, uv.y + 1.0f / ATLAS_SIZE);
                        vertex(1.0f, 1.0f, 0.0f, uv.x + 1.0f / ATLAS_SIZE, uv.y);
                        break;
                    // Y-
                    case 3:
                        vertex(0.0f, 0.0f, 0.0f, uv.x, uv.y);
                        vertex(0.0f, 0.0f, 1.0f, uv.x, uv.y + 1.0f / ATLAS_SIZE);
                        vertex(1.0f, 0.0f, 1.0f, uv.x + 1.0f / ATLAS_SIZE, uv.y + 1.0f / ATLAS_SIZE);
                        vertex(1.0f, 0.0f, 0.0f, uv.x + 1.0f / ATLAS_SIZE, uv.y);
                        break;
                    // Z+
                    case 4:
                        vertex(0.0f, 0.0f, 1.0f, uv.x, uv.y);
                        vertex(0.0f, 1.0f, 1.0f, uv.x, uv.y + 1.0f / ATLAS_SIZE);
                        vertex(1.0f, 1.0f, 1.0f, uv.x + 1.0f / ATLAS_SIZE, uv.y + 1.0f / ATLAS_SIZE);
                        vertex(1.0f, 0.0f, 1.0f, uv.x + 1.0f / ATLAS_SIZE, uv.y);
                        break;
                    // Z-
                    case 5:
                        vertex(0.0f, 0.0f, 0.0f, uv.x, uv.y);
                        vertex(0.0f, 1.0f, 0.0f, uv.x, uv.y + 1.0f / ATLAS_SIZE);
                        vertex(1.0f, 1.0f, 0.0f, uv.x + 1.0f / ATLAS_SIZE, uv.y + 1.0f / ATLAS_SIZE);
                        vertex(1.0f, 0.0f, 0.0f, uv.x + 1.0f / ATLAS_SIZE, uv.y);
                        break;
                }

                // direct and reverse order (when polygon must be rendered from other side)
                if (face % 2 == 0){
                    index(0, 1, 3, 1, 2, 3);
                } else {
                    index(3, 1, 0, 3, 2, 1);
                }
                }
            }
        }
    }
}