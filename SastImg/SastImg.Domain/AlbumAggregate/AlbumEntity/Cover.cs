using SastImg.Domain.AlbumAggregate.ImageEntity;

namespace SastImg.Domain.AlbumAggregate.AlbumEntity
{
    public sealed record class Cover(Uri? Url, bool IsLatestImage)
    {
        public Cover(ImageUrl? url, bool isLatestImage)
            : this(url?.Thumbnail, isLatestImage) { }

        public static bool operator ==(Cover cover, ImageUrl image) => image.Thumbnail == cover.Url;

        public static bool operator !=(Cover cover, ImageUrl image) => !(cover == image);
    };
}
