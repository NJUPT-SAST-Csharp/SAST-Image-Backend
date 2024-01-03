using Shared.Primitives.DomainEvent;

namespace SastImg.Domain.AlbumAggregate.AlbumEntity.Events
{
    public sealed class AddImageDomainEvent(long albumId, long authorId, long imageId)
        : IDomainEvent
    {
        public long AlbumId { get; } = albumId;
        public long AuthorId { get; } = authorId;
        public long ImageId { get; } = imageId;
    }
}
