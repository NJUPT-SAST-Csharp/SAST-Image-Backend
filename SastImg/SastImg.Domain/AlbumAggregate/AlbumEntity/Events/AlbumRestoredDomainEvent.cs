using Shared.Primitives.DomainEvent;

namespace SastImg.Domain.AlbumAggregate.AlbumEntity.Events
{
    public sealed class AlbumRestoredDomainEvent(AlbumId albumId) : IDomainEvent
    {
        public AlbumId AlbumId { get; } = albumId;
    }
}
