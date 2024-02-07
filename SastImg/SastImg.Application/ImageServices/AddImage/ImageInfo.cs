namespace SastImg.Application.ImageServices.AddImage
{
    public sealed class ImageInfo(Uri url)
    {
        public Uri Url { get; init; } = url;
    }
}
