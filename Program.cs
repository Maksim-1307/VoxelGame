using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace VoxelGame
{
    public static class Program
    {
        private static void printArr(int[] arr){
            Console.WriteLine("Array output: ");
            for (int i = 0; i < 5; i++){
                Console.WriteLine(arr[i]);
            }
        }
        private static void Main()
        {
            var nativeWindowSettings = new NativeWindowSettings()
            {
                ClientSize = new Vector2i(800, 600),
                Title = "VoxelGame",
                Flags = ContextFlags.ForwardCompatible,
            };

            using (var window = new Window(GameWindowSettings.Default, nativeWindowSettings))
            {
                window.Run();
            }
        }
    }
}