using Domain.AlbumAggregate.AlbumEntity;
using Domain.Event;

namespace Domain.AlbumAggregate.Events;

public sealed record class AlbumDescriptionUpdatedEvent(AlbumId Album, AlbumDescription Description)
    : IDomainEvent { }
