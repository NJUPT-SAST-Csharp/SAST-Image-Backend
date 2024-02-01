using SastImg.Domain.AlbumAggregate.ImageEntity;
using Shared.Primitives.DomainEvent;

namespace SastImg.Domain.AlbumAggregate.AlbumEntity.Events
{
    public sealed class ImageAddedDomainEvent(
        in AlbumId albumId,
        in UserId authorId,
        in ImageId imageId
    ) : IDomainEvent
    {
        public AlbumId AlbumId { get; } = albumId;
        public UserId AuthorId { get; } = authorId;
        public ImageId ImageId { get; } = imageId;
    }
}
