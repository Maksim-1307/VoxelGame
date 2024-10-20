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

        private Vector3 _position;

        private bool _buffersLoaded = false;
        
        public MeshRenderer(Camera Camera){
            _camera = Camera;
        }

        public void setMesh(Mesh Mesh){
            _mesh = Mesh;
        }
        public void setShader(Shader Shader){
            _shader = Shader;
        }
        public void setTexture(Texture Texture) {
            _texture = Texture;
            _texture2 = Texture;
        }
        public void setPosition(Vector3 Position){
            _position = Position;
        }

        public void RenderAt(float x, float y, float z){

            _texture.Use(TextureUnit.Texture0);
            _texture2.Use(TextureUnit.Texture1);
            _shader.Use();

            var model = Matrix4.Identity * Matrix4.CreateTranslation(x, y, z);
            _shader.SetMatrix4("model", model);
            _shader.SetMatrix4("view", _camera.GetViewMatrix());
            _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());

            _mesh.draw();
        }

        public void Render(){
            RenderAt(_position.X, _position.Y, _position.Z);
        }
    
    }
}