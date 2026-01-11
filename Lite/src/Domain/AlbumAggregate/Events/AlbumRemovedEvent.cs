using Domain.AlbumAggregate.AlbumEntity;
using Domain.Event;

namespace Domain.AlbumAggregate.Events;

public sealed record class AlbumRemovedEvent(AlbumId Album, AlbumStatus Status) : IDomainEvent { }
