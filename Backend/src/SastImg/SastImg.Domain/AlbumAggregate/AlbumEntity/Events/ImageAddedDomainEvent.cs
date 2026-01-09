using Identity;
using Mediator;
using SastImg.Domain.AlbumAggregate.ImageEntity;

namespace SastImg.Domain.AlbumAggregate.AlbumEntity.Events;

public sealed class ImageAddedDomainEvent(AlbumId albumId, UserId authorId, ImageId imageId)
    : IDomainEvent
{
    public AlbumId AlbumId { get; } = albumId;
    public UserId AuthorId { get; } = authorId;
    public ImageId ImageId { get; } = imageId;
}
