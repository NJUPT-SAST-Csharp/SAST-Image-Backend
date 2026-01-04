using SastImg.Domain.AlbumAggregate.ImageEntity;

namespace SastImg.Application.ImageServices.AddImage;

public sealed class ImageInfoDto(ImageId id)
{
    public long Id { get; } = id.Value;
}
