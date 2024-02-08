namespace SastImg.Application.ImageServices.AddImage
{
    public sealed class ImageInfoDto(Uri url)
    {
        public Uri Url { get; init; } = url;
    }
}
