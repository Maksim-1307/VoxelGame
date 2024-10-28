namespace VoxelGame.Voxels
{
    public class Block
    {
        public static Dictionary<byte, Block> blocks = new Dictionary<byte, Block>();
        public byte voxelId;
        public BlockModel blockModel;
        public (uint x, uint y) [] UVs;
        public bool [] openFaces = new bool [6];

        public Block(byte voxelId, BlockModel blockModel, (uint x, uint y)[] UVs)
        {
            this.voxelId = voxelId;
            this.blockModel = blockModel;
            this.UVs = UVs;
            if (UVs.Count() == 0) throw new ArgumentException("Block must have at least one texture UV");

            blocks.Add(voxelId, this);
        }

        public static Block GetBlockByVoxelId(byte id){
            if (!blocks.ContainsKey(id)) return blocks[0];
            return blocks[id];
        }

        public (uint x, uint y) GetUV(byte face){
            if (face < 0 || face > 5) throw new ArgumentException("Face is a number between 0 and 6");
            switch (UVs.Count()){
                case 1:
                    return UVs[0];
                case 2:
                    // like a log block. Top and bottom are texture 0, sides are texture 1
                    if (face == 2 || face == 3) return UVs[0];
                    return UVs[1];
                case 3:
                    // like a grass block 
                    if (face == 2) return UVs[0]; // top face
                    if (face == 3) return UVs[1]; // bottom face
                    return UVs[2];                // side face
            }
            return UVs[0];
        }
    }

    public enum BlockModel {
        Air, Cube, XSprite, Stairs, HalfBlock, Fluid
    }
}