using VoxelGame.Graphics;
using VoxelGame.Logic;

namespace VoxelGame.Lighting
{
    public class LightMap 
    {
        private const int Width = 16;
        private const int Height = 256;
        private const int Depth = 16;
        public int highestPoint = 0;

        private Light[,,] lights;

        public LightMap()
        {
            lights = new Light[Width, Height, Depth];
            for (int x = 0; x < Width; x++){
                for (int z = 0; z < Width; z++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        lights[x, y, z] = new Light(0);
                    }
                }
            }
        }


        public void SetLight(int x, int y, int z, Light light)
        {
            if (IsInBounds(x, y, z))
            {
                lights[x, y, z] = light;
            }
            else
            {
                // Console.WriteLine(x + " " + y + " " + z);
                // throw new ArgumentOutOfRangeException("Coordinates out of bounds.");
            }
        }

        public Light GetLight(int x, int y, int z)
        {
            if (IsInBounds(x, y, z))
            {
                return lights[x, y, z];
            }
            else
            {
                return new Light(0);
                //throw new ArgumentOutOfRangeException("Coordinates out of bounds.");
            }
        }

        private bool IsInBounds(int x, int y, int z)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height && z >= 0 && z < Depth;
        }
    }
}