namespace SastImg.Application.ImageServices.GetImage
{
    public sealed class DetailedImageDto
    {
        private DetailedImageDto() { }

        public required long ImageId { get; init; }
        public required string Title { get; init; }
        public required DateTime UploadedAt { get; init; }
        public required long[] Tags { get; init; }
        public required Uri Url { get; init; }
    }
}
