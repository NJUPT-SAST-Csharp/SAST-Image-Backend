using Domain.AlbumAggregate.AlbumEntity;
using Domain.Event;

namespace Domain.AlbumAggregate.Events;

public sealed record class AlbumTagsUpdatedEvent(AlbumId Id, AlbumTags Tags) : IDomainEvent;
