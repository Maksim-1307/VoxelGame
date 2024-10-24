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
        private Font _font;

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
            _font = new Font("path");
            _mesh = genMeshOfCharacter('1');
        }

        public override void Update(){
            _fontTexture.Use(TextureUnit.Texture0);
            _shader.Use();
            _mesh.draw();
        }

        public Mesh genMeshOfCharacter(char character){

            Character ch = _font.getCharacter(character);

            int texWidth = 512;
            int texHeight = 512;
            int lineNum = ch.line;
            int w0 = ch.width;
            int lh = 11;
            int posX = ch.posX;
            int posY = lineNum * lh;
            float fz = 10.0f;

            (float x, float y) pos = ((float)posX / (float)texWidth, (float)posY / (float)texHeight * -1);
            float w = (float)w0 / (float)texWidth;
            float h = (float)lh / (float)texHeight;
            return new Mesh(
                [
                    0.0f, 0.0f, 0.0f, 0.0f + pos.x, 1.0f - h + pos.y,
                    w * fz, 0.0f, 0.0f, w    + pos.x, 1.0f - h + pos.y,
                    0.0f, h * fz, 0.0f, 0.0f + pos.x, 1.0f     + pos.y, 
                    w * fz, h * fz, 0.0f, w    + pos.x, 1.0f     + pos.y
                ],
                [
                    0, 1, 3, 0, 3, 2
                ]
            );
        }
        
        
    }

    public class Font {

        private Character [] CharactersData = [
            new Character('a', 5, 0, 0),
            new Character('b', 5, 5, 0),
            new Character('C', 6, 12, 1),
            new Character('1', 3, 5, 2)
        ];
        private Character nullCharacter = new Character(' ', 5, 0, 0);

        public Font (string path) {

        }


        public Character getCharacter(char name){
            for (int i = 0; i < CharactersData.Count(); i++){
                if (CharactersData[i].name == name){
                    return CharactersData[i];
                }
            }
            return nullCharacter;
        }
    }

    public class Character {

        public char name;
        public int width;
        public int line;
        public int posX;

        public Character(char name, int width, int posX, int line){
            this.name = name;
            this.width = width;
            this.line = line;
            this.posX = posX;
        }
    }
}