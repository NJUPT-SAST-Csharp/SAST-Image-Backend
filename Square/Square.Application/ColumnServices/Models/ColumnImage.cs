namespace Square.Application.ColumnServices.Models
{
    public sealed class ColumnImage(Uri url, Uri thumbnailUrl)
    {
        public Uri Url { get; init; } = url;
        public Uri ThumbnailUrl { get; init; } = thumbnailUrl;
    }
}
