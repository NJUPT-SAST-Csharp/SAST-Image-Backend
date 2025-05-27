using Mediator;

namespace SastImg.Domain.AlbumAggregate.AlbumEntity.Events;

public sealed class AlbumRestoredDomainEvent(in AlbumId albumId) : IDomainEvent
{
    public AlbumId AlbumId { get; } = albumId;
}
