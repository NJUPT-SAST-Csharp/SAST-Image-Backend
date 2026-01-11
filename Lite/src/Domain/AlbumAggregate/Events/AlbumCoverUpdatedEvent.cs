using Domain.AlbumAggregate.AlbumEntity;
using Domain.AlbumAggregate.ImageEntity;
using Domain.Event;
using Domain.Shared;

namespace Domain.AlbumAggregate.Events;

public sealed record class AlbumCoverUpdatedEvent(
    AlbumId Album,
    ImageId? ContainedImage,
    IImageFile? CoverImage
) : IDomainEvent
{
    public static AlbumCoverUpdatedEvent ContainedImageOrEmpty(
        AlbumId album,
        ImageId? containedImage
    ) => new(album, containedImage, null);

    public static AlbumCoverUpdatedEvent UserCustomImage(AlbumId album, IImageFile coverImage) =>
        new(album, null, coverImage);

    public static AlbumCoverUpdatedEvent NewAddedImage(AlbumId album, IImageFile imageFile) =>
        new(album, null, imageFile);
}
