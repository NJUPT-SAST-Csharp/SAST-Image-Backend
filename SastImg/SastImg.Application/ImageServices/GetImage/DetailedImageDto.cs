namespace SastImg.Application.ImageServices.GetImage
{
    public sealed class DetailedImageDto
    {
        private DetailedImageDto() { }

        public long ImageId { get; init; }
        public string Title { get; init; }
        public DateTime UploadedAt { get; init; }
        public bool IsRemoved { get; init; }
        public long[] Tags { get; init; }
        public Uri Url { get; init; }
    }
}
