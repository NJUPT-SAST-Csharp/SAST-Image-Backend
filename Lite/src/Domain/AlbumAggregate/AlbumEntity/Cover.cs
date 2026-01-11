using Domain.AlbumAggregate.ImageEntity;

namespace Domain.AlbumAggregate.AlbumEntity;

public sealed record class Cover(ImageId? Id, bool IsLatestImage)
{
    public static readonly Cover Default = new(null, true);
    public static readonly Cover UserCustomCover = new(null, false);
}
