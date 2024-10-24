using System;
using System.IO;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Input;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Graphics.OpenGL4;
using StbImageSharp;
using OpenTK.Mathematics;
using System.Collections.Generic;

using VoxelGame.Logic;
using VoxelGame.Graphics;
using VoxelGame.Voxels;

namespace VoxelGame.Graphics{
    public class Text : GameObject{

        private Texture _fontTexture;
        private Shader _shader;
        private string _text;
        private Mesh _mesh;

        private const int lineHeight = 12;

        // private float [] vertices = [
        //     0.0f, 0.0f, 0.0f, 0.0f, 0.0f,
        //     0.5f, 0.0f, 0.0f, 1.0f, 0.0f,
        //     0.0f, 0.5f, 0.0f, 0.0f, 1.0f,
        //     0.5f, 0.5f, 0.0f, 1.0f, 1.0f
        // ];
        // private uint [] indices = [
        //     0, 1, 3, 0, 3, 2
        // ];

        public Text(string text) {
            _text = text;
            _fontTexture = Texture.LoadFromFile("res/textures/lightbulb.png");
            _shader = new Shader("res/shaders/fontShader.vert", "res/shaders/shader.frag");
            _mesh = genMeshOfCharacter();
        }

        public override void Update(){
            _fontTexture.Use(TextureUnit.Texture0);
            _shader.Use();
            _mesh.draw();
        }

        public Mesh genMeshOfCharacter(){
            int texWidth = 512;
            int texHeight = 512;
            (float x, float y) pos = (0, (texHeight - lineHeight * 1));
            int w = 5;
            return new Mesh(
                [
                    0.0f, 0.0f, 0.0f, 0.0f, 0.6f,
                    0.5f, 0.0f, 0.0f, 0.4f, 0.6f,
                    0.0f, 0.5f, 0.0f, 0.0f, 1.0f,
                    0.5f, 0.5f, 0.0f, 0.4f, 1.0f
                ],
                [
                    0, 1, 3, 0, 3, 2
                ]
            );
        }
        
        
    }
}