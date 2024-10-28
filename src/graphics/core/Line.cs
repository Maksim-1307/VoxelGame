using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace VoxelGame.Graphics{
    public class Line {

        private Shader _shader;

        Vector3 _color;
        Mesh _mesh;
        public Line(Vector3 startPos, Vector3 endPos, Vector3 color){
            _mesh = new Mesh([
                startPos.X, startPos.Y, startPos.Z,
                endPos.X, endPos.Y, endPos.Z
            ], []);
            _mesh.loadBuffers();
            _color = color;
            _shader = new Shader("res/shaders/line.vert", "res/shaders/line.frag");
        }
        public void Draw(Camera camera){
            _shader.Use();
            _shader.SetMatrix4("projection", camera.GetProjectionMatrix());
            _shader.SetMatrix4("view", camera.GetViewFromNullMatrix());
            _shader.SetVector3("color", _color);

            GL.BindVertexArray(_mesh.VAO);
            GL.DrawArrays(PrimitiveType.Lines, 0, 2);
            GL.BindVertexArray(0);
        }
    }
}
/*
class Line
{
    int shaderProgram;
    unsigned int VBO, VAO;
    vector<float> vertices;
    vec3 startPoint;
    vec3 endPoint;
    mat4 MVP;
    vec3 lineColor;
    public:
    Line(vec3 start, vec3 end)
    {

        startPoint = start;
        endPoint = end;
        lineColor = vec3(1, 1, 1);
        MVP = mat4(1.0f);

        const char* vertexShaderSource = ;
        const char* fragmentShaderSource = ;

        // vertex shader
        int vertexShader = glCreateShader(GL_VERTEX_SHADER);
        glShaderSource(vertexShader, 1, &vertexShaderSource, NULL);
        glCompileShader(vertexShader);
        // check for shader compile errors

        // fragment shader
        int fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
        glShaderSource(fragmentShader, 1, &fragmentShaderSource, NULL);
        glCompileShader(fragmentShader);
        // check for shader compile errors

        // link shaders
        shaderProgram = glCreateProgram();
        glAttachShader(shaderProgram, vertexShader);
        glAttachShader(shaderProgram, fragmentShader);
        glLinkProgram(shaderProgram);
        // check for linking errors

        glDeleteShader(vertexShader);
        glDeleteShader(fragmentShader);

        vertices = {
            start.x, start.y, start.z,
             end.x, end.y, end.z,

        };

        glGenVertexArrays(1, &VAO);
        glGenBuffers(1, &VBO);
        glBindVertexArray(VAO);

        glBindBuffer(GL_ARRAY_BUFFER, VBO);
        glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), vertices.data(), GL_STATIC_DRAW);

        glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 3 * sizeof(float), (void*)0);
        glEnableVertexAttribArray(0);

        glBindBuffer(GL_ARRAY_BUFFER, 0);
        glBindVertexArray(0);

    }

    int setMVP(mat4 mvp)
    {
        MVP = mvp;
        return 1;
    }

    int setColor(vec3 color)
    {
        lineColor = color;
        return 1;
    }

    int draw()
    {

        glUseProgram(shaderProgram);
        glUniformMatrix4fv(glGetUniformLocation(shaderProgram, "MVP"), 1, GL_FALSE, &MVP[0][0]);
        glUniform3fv(glGetUniformLocation(shaderProgram, "color"), 1, &lineColor[0]);

        glBindVertexArray(VAO);
        glDrawArrays(GL_LINES, 0, 2);
        return 1;
    }

    ~Line()
    {

        glDeleteVertexArrays(1, &VAO);
        glDeleteBuffers(1, &VBO);
        glDeleteProgram(shaderProgram);
    }
};

*/