using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace VoxelGame.Graphics
{
    public class AxisLines : UIelement {

        private Line XLine;
        private Line YLine;
        private Line ZLine;
        public float lineLen = 0.1f;

        public AxisLines () {
            XLine = new Line(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 0.0f, 0.0f) * lineLen, new Vector3(1.0f, 0.0f, 0.0f));
            YLine = new Line(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f) * lineLen, new Vector3(0.0f, 1.0f, 0.0f));
            ZLine = new Line(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 1.0f) * lineLen, new Vector3(0.0f, 0.0f, 1.0f));
        }

        public override void Draw(Camera camera)
        {
            XLine.Draw(camera);
            YLine.Draw(camera);
            ZLine.Draw(camera);
        }
    }
}