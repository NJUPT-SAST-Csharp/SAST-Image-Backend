using Shared.Primitives.DomainEvent;

namespace SastImg.Domain.AlbumAggregate.AlbumEntity.Events
{
    public sealed class AlbumRemovedDomainEvent(AlbumId albumId) : IDomainEvent
    {
        public AlbumId AlbumId { get; } = albumId;
    }
}
