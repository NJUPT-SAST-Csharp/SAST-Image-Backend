using Shared.Primitives.DomainEvent;

namespace SastImg.Domain.AlbumAggregate.AlbumEntity.Events
{
    public sealed class AlbumCreatedDomainEvent(AlbumId albumId, UserId authorId) : IDomainEvent
    {
        public AlbumId AlbumId { get; } = albumId;
        public UserId AuthorId { get; } = authorId;
    }
}
