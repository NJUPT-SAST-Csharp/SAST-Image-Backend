using SastImg.Domain.AlbumAggregate.ImageEntity;

namespace SastImg.Domain.AlbumAggregate.AlbumEntity;

public sealed record class Cover(ImageId? ImageId, bool IsLatestImage)
{
    public static bool operator ==(Cover cover, Image image) => image.Id == cover.ImageId;

    public static bool operator ==(Image image, Cover cover) => image.Id == cover.ImageId;

    public static bool operator !=(Cover cover, Image image) => !(cover == image);

    public static bool operator !=(Image image, Cover cover) => !(image == cover);

    public static readonly Cover Empty = new(null, true);
}
