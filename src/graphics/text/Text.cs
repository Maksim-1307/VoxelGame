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
            _fontTexture = Texture.LoadFromFile("res/textures/font.png");
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
            int w0 = 5;
            int lh = 11;
            (float x, float y) pos = (0, 0);
            float w = (float)w0 / (float)texWidth;
            float h = (float)lh / (float)texHeight;
            Console.WriteLine(h + " sdfdsfsd");
           // w = 0.1f;
            //h = 0.05f;
            return new Mesh(
                [
                    0.0f, 0.0f, 0.0f, 0.0f, 1.0f - h,
                    0.5f, 0.0f, 0.0f, w, 1.0f - h,
                    0.0f, 0.5f, 0.0f, 0.0f, 1.0f, 
                    0.5f, 0.5f, 0.0f, w, 1.0f
                ],
                [
                    0, 1, 3, 0, 3, 2
                ]
            );
        }
        
        
    }
}