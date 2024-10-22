using VoxelGame.Graphics;
using VoxelGame.Logic;

namespace VoxelGame.Voxels{
    public class Chunk : GameObject
{
    private const int Width = 16;
    private const int Height = 256;
    private const int Depth = 16;

    private Voxel[,,] blocks;

    public Chunk()
    {
        blocks = new Voxel[Width, Height, Depth];
    }

    public override void Update() {

    }

    public void SetVoxel(int x, int y, int z, Voxel voxel)
    {
        if (IsInBounds(x, y, z))
        {
            blocks[x, y, z] = voxel;
        }
        else
        {
            throw new ArgumentOutOfRangeException("Coordinates out of bounds.");
        }
    }

    public Voxel GetVoxel(int x, int y, int z)
    {
        if (IsInBounds(x, y, z))
        {
            return blocks[x, y, z]; 
        }
        else
        {
            return new Voxel(0,0);
            //throw new ArgumentOutOfRangeException("Coordinates out of bounds.");
        }
    }

    private bool IsInBounds(int x, int y, int z)
    {
        return x >= 0 && x < Width && y >= 0 && y < Height && z >= 0 && z < Depth;
    }

    public void Render(){

    }
}
}