namespace SastImg.Application.ImageServices.GetImages
{
    public sealed class ImageDto(long id, string title, Uri url)
    {
        public long ImageId { get; init; } = id;
        public string Title { get; init; } = title;
        public bool IsNsfw { get; init; } = false;
        public Uri Url { get; init; } = url;
    }
}
