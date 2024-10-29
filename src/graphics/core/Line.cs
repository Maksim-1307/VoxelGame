using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace VoxelGame.Graphics{
    public class Line {

        private Shader _shader;
        Vector3 _color;
        public int VBO;
        public int VAO;
        private float [] vertices;
        private bool buffersLoaded = false;
        public Line(Vector3 startPos, Vector3 endPos, Vector3 color){
            vertices = [
                startPos.X, startPos.Y, startPos.Z,
                endPos.X, endPos.Y, endPos.Z, 
            ];
            _color = color;
            _shader = new Shader("res/shaders/line.vert", "res/shaders/line.frag");
        }
        public void loadBuffers(){
            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            var vertexLocation = 0;
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            buffersLoaded = true;
        }
        public void Draw(Camera camera){
            if (!buffersLoaded) loadBuffers();
            _shader.Use();
            _shader.SetMatrix4("projection", camera.GetProjectionMatrix());
            _shader.SetMatrix4("view", camera.GetViewFromNullMatrix());
            _shader.SetVector3("color", _color);

            GL.BindVertexArray(VAO);
            GL.DrawArrays(PrimitiveType.Lines, 0, 2);
            GL.BindVertexArray(0);
        }
    }
}
