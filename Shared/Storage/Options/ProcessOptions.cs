namespace Storage.Options
{
    public readonly struct ProcessOptions()
    {
        public int Width { get; init; } = 0;
        public int Height { get; init; } = 0;
        public float ResizeRatio { get; init; } = 1f;
        public bool CompressToWebp { get; init; } = false;
    }
}
