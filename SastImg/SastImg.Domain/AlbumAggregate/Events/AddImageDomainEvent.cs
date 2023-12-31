using Primitives.DomainEvent;

namespace SastImg.Domain.AlbumAggregate.Events
{
    public sealed class AddImageDomainEvent(long albumId, long authorId, long imageId)
        : DomainEventBase
    {
        public long AlbumId { get; } = albumId;
        public long AuthorId { get; } = authorId;
        public long ImageId { get; } = imageId;
    }
}
