namespace VoxelGame.Lighting
{
    public struct Light
    {
        public byte Value { get; set; }

        public Light(byte value)
        {
            Value = value;
        }
    }
}