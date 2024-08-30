namespace VoxelGame.Voxels{
    public struct Voxel
    {
        public byte Id { get; set; }
        public byte State { get; set; }

        public Voxel(byte id, byte state)
        {
            Id = id;
            State = state;
        }
    }
}