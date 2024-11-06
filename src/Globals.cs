using VoxelGame.Graphics;
using VoxelGame.Logic;
using VoxelGame.Voxels;
using VoxelGame.Lighting;
using VoxelGame.Time;
using VoxelGame.World;

namespace VoxelGame
{
    public static class Globals{
        public static Generator generator;
        public static VoxelStorage voxelStorage;
        public static Lighting.Lighting lighting;
        public static Camera camera;
        public static LightSolver lightSolver;
        public static Canvas canvas;
        public static FPSCounter fpsCounter;
        public static ChunksController chunksController;
        
        public static void Init(){
            return;
        }
    }
}