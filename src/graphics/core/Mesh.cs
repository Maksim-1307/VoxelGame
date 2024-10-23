using System;
using System.IO;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Graphics.OpenGL4;

namespace VoxelGame.Graphics {
    public class Mesh {
        public readonly float[] vertices;
        public readonly uint[] indices;

        public int EBO;
        public int VBO;
        public int VAO;

        public bool buffersLoaded = false;

        public Mesh(float[] vertices, uint[] indices) {
            this.vertices = vertices;
            this.indices = indices;
            //loadBuffers();
        }

        public void draw(){
            if (!buffersLoaded) loadBuffers();
            GL.BindVertexArray(VAO);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
        }

        private void loadBuffers(){
            //Console.WriteLine("loadBuffers");
            VAO = GL.GenVertexArray();
            //Console.WriteLine("loadBuffers");
            GL.BindVertexArray(VAO);

            //Console.WriteLine("loadBuffers");

            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            //Console.WriteLine("loadBuffers");

            EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            //Console.WriteLine("loadBuffers");

            var vertexLocation = 0;
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            //Console.WriteLine("loadBuffers");

            var texCoordLocation = 1;
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            //Console.WriteLine("LoadBuffers end");
            buffersLoaded = true;
        }
    }
}