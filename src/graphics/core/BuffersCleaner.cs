using System.Collections.Generic;
using System.Collections.Concurrent;
using OpenTK.Graphics.OpenGL4;

namespace VoxelGame.Graphics
{
    public class BuffersCleaner {
        private static Queue<int> VertexArrays = new Queue<int>();
        private static Queue<int> VertexBuffers = new Queue<int>();
        public static void AddToQueue(BufferType type, int buffer){
            switch (type) {
                case BufferType.VertexArray:
                    VertexArrays.Enqueue(buffer);
                    break;
                case BufferType.VertexBuffer:
                    VertexBuffers.Enqueue(buffer);
                    break;
            }
        }

        // should be called in OnUpdateFrame  
        public static void Clean(){
            while (VertexArrays.Count > 0)
            {
                int buff = VertexArrays.Dequeue();
                GL.DeleteVertexArray(buff);
            }
            while (VertexBuffers.Count > 0)
            {
                int buff = VertexBuffers.Dequeue();
                GL.DeleteBuffer(buff);
            }
        }
    }

    public enum BufferType {
        VertexArray, VertexBuffer
    }
}