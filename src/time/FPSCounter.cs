using System;
using System.Diagnostics;
using VoxelGame.Logic;
using VoxelGame.Graphics;
using OpenTK.Graphics.ES11;

// нужно вынести логику отрисовки fps на экране в отдельный класс. Тут только подсчет fps

namespace VoxelGame.Time{

    public class FPSCounter : GameObject{

        private uint fps = 0;
        private uint minFps = 0;
        private uint frames = 0;
        private float totalTime = 0.0f;
        private Text fpsText;

        public FPSCounter(){            
            //fpsText = new Text("fps " + fps);
        }

        public override void Update(float deltaTime){
            frames++;
            totalTime += deltaTime;
            minFps = (uint)MathF.Min(minFps, 1.0f / deltaTime);
            //if (stopwatch.Elapsed.TotalSeconds >= 1.0f)
            if (totalTime >= 1.0f)
            {
                fps = frames;
                Console.WriteLine(fps + " | " + minFps);
                //fpsText.Update("fps " + fps);
                frames = 0;
                minFps = fps;
                totalTime = 0.0f;
                //stopwatch.Restart();
            }
        }

        public uint getFps(){
            return fps;
        }


    }
}