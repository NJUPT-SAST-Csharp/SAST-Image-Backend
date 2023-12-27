namespace SastImg.Application.ImageServices.GetDetailedImage
{
    public sealed class DetailedImage
    {
        private DetailedImage() { }

        public required long ImageId { get; init; }
        public required string Title { get; init; }
        public required bool IsNsfw { get; init; }
        public required DateTime UploadedAt { get; init; }
        public required IEnumerable<long> Tags { get; init; }
        public required Uri Url { get; init; }
    }
}
