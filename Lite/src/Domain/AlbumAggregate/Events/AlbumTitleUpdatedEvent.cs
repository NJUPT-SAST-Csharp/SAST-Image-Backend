using Domain.AlbumAggregate.AlbumEntity;
using Domain.Event;

namespace Domain.AlbumAggregate.Events;

public sealed record class AlbumTitleUpdatedEvent(AlbumId Album, AlbumTitle Title)
    : IDomainEvent { }
