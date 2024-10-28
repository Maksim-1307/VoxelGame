namespace VoxelGame.Voxels
{
    public class Block
    {
        public static Dictionary<byte, Block> blocks = new Dictionary<byte, Block>();
        public byte voxelId;
        public BlockModel blockModel;
        public (uint x, uint y) uvPos;

        public Block(byte voxelId, BlockModel blockModel, (uint x, uint y) uvPos)
        {
            this.voxelId = voxelId;
            this.blockModel = blockModel;
            this.uvPos = uvPos;

            blocks.Add(voxelId, this);
        }

        public static Block GetBlockByVoxelId(byte id){
            if (!blocks.ContainsKey(id)) return blocks[0];
            return blocks[id];
        }
    }

    public enum BlockModel {
        Air, Cube, XSprite, Stairs, HalfBlock, Fluid
    }
}