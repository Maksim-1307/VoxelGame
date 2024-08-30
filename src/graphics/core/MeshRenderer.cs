using System;
using System.IO;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Input;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Graphics.OpenGL4;
using StbImageSharp;
using OpenTK.Mathematics;

namespace VoxelGame.Graphics{

    public class MeshRenderer {
        private Mesh _mesh;
        private Shader _shader;
        private Texture _texture;
        private Texture _texture2;
        public Camera _camera;

        private int _EBO;
        private int _VBO;
        private int _VAO;
        
        public MeshRenderer(Camera Camera){
            _camera = Camera;
        }

        private bool _buffersLoaded = false;

        public void setMesh(Mesh Mesh){
            _mesh = Mesh;
            _buffersLoaded = false;
        }
        public void setShader(Shader Shader){
            _shader = Shader;
        }
        public void setTexture(Texture Texture) {
            _texture = Texture;
            _texture2 = Texture;
        }

        public void Render(){
            if (!_buffersLoaded) loadBuffers();

            GL.BindVertexArray(_VAO);

            _texture.Use(TextureUnit.Texture0);
            _texture2.Use(TextureUnit.Texture1);
            _shader.Use();

            var model = Matrix4.Identity * Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(1.0f));
            _shader.SetMatrix4("model", model);
            _shader.SetMatrix4("view", _camera.GetViewMatrix());
            _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());

            GL.DrawElements(PrimitiveType.Triangles, _mesh.indices.Length, DrawElementsType.UnsignedInt, 0);
        }
        
        private void loadBuffers(){
            _VAO = GL.GenVertexArray();
            GL.BindVertexArray(_VAO);

            _VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, _mesh.vertices.Length * sizeof(float), _mesh.vertices, BufferUsageHint.StaticDraw);

            _EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _mesh.indices.Length * sizeof(uint), _mesh.indices, BufferUsageHint.StaticDraw);

            var vertexLocation = 0;
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            var texCoordLocation = 1;
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            _buffersLoaded = true;
        }
    }
}