using OpenTK.Mathematics;

using VoxelGame.Graphics;
using VoxelGame.Logic;
using VoxelGame.Voxels;
using VoxelGame.Lighting;
using VoxelGame.Time;
using VoxelGame.World;

namespace VoxelGame
{
    public static class Globals{
        public static Window window;
        public static Generator generator;
        public static VoxelStorage voxelStorage;
        public static Lighting.Lighting lighting;
        public static Camera camera;
        public static LightSolver lightSolver;
        public static Canvas canvas;
        public static FPSCounter fpsCounter;
        public static ChunksController chunksController;
        
        public static void Init(){
            Globals.generator = new Generator();
            Globals.voxelStorage = new VoxelStorage();
            Globals.lighting = new Lighting.Lighting(voxelStorage);
            Globals.lightSolver = new LightSolver(0);
            Globals.camera = new Camera(Vector3.UnitZ * 3, window.Size.X / (float)window.Size.Y);
            Globals.chunksController = new ChunksController();
            Globals.canvas = new Canvas();
        }
    }
}