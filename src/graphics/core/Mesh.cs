namespace VoxelGame.Graphics {
    public class Mesh {
        public readonly float[] vertices;
        public readonly uint[] indices;

        public Mesh(float[] vertices, uint[] indices) {
            this.vertices = vertices;
            this.indices = indices;
        }
    }
}