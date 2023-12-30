namespace SastImg.Application.ImageServices.GetImages
{
    public sealed class ImageDto
    {
        private ImageDto() { }

        public required long ImageId { get; init; }
        public required string Title { get; init; }
        public required DateTime UploadedAt { get; init; }
        public required Uri Url { get; init; }
    }
}
