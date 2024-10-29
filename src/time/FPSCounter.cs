using System;
using System.Diagnostics;
using VoxelGame.Logic;
using VoxelGame.Graphics;

// нужно вынести логику отрисовки fps на экране в отдельный класс. Тут только подсчет fps

namespace VoxelGame.Time{

    public class FPSCounter : GameObject{

        private uint fps = 0;
        private uint frames = 0;
        private Stopwatch stopwatch;
        private Text fpsText;

        public FPSCounter(){            
            stopwatch = new Stopwatch();
            stopwatch.Start();
            fpsText = new Text("fps " + fps);
        }

        public override void Update(){
            frames++;
            if (stopwatch.Elapsed.TotalSeconds >= 1.0f)
            {
                fps = frames;
                fpsText.Update("fps " + fps);
                frames = 0;
                stopwatch.Restart();
            }
        }

        public uint getFps(){
            return fps;
        }


    }
}