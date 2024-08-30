using System;
using System.Diagnostics;

namespace VoxelGame.Time{

    public class FPSCounter{

        private uint frames = 0;
        private Stopwatch stopwatch;

        public FPSCounter(){            
            stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        public void Update(){
            frames++;

            if (stopwatch.Elapsed.TotalSeconds >= 1.0f)
            {
                Console.WriteLine(frames);
                frames = 0;
                stopwatch.Restart();
            }
        }


    }
}