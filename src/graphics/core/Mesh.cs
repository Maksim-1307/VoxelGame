using System;
using System.IO;
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
        }
        ~Mesh(){
            BuffersCleaner.AddToQueue(BufferType.VertexArray, this.VAO);
            BuffersCleaner.AddToQueue(BufferType.VertexBuffer, this.VBO);
            BuffersCleaner.AddToQueue(BufferType.VertexBuffer, this.EBO);
        }

        public void Update(float[] newVertices, uint[] newIndices)
        {

        }

        public void draw(){
            if (!buffersLoaded) loadBuffers();
            GL.BindVertexArray(VAO);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
        }

        public void loadBuffers(){
            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            var vertexLocation = 0;
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

            var texCoordLocation = 1;
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

            var colorLocation = 2;
            GL.EnableVertexAttribArray(colorLocation);
            GL.VertexAttribPointer(colorLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 5 * sizeof(float));
            buffersLoaded = true;
        }
    }
}