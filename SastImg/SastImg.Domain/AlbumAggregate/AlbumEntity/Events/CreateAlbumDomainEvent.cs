using Shared.Primitives.DomainEvent;

namespace SastImg.Domain.AlbumAggregate.AlbumEntity.Events
{
    public sealed class CreateAlbumDomainEvent(long albumId, long authorId) : IDomainEvent
    {
        public long AlbumId { get; } = albumId;
        public long AuthorId { get; } = authorId;
    }
}
