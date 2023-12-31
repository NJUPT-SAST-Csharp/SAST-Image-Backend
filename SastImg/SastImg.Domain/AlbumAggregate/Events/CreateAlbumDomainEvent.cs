using Primitives.DomainEvent;

namespace SastImg.Domain.AlbumAggregate.Events
{
    public sealed class CreateAlbumDomainEvent(long albumId, long authorId) : DomainEventBase
    {
        public long AlbumId { get; } = albumId;
        public long AuthorId { get; } = authorId;
    }
}
