using Domain.AlbumAggregate.AlbumEntity;
using Domain.Event;

namespace Domain.AlbumAggregate.Events;

public sealed record class AlbumAccessLevelUpdatedEvent(AlbumId Album, AccessLevel AccessLevel)
    : IDomainEvent { }
