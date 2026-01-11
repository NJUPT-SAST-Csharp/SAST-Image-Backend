using Domain.AlbumAggregate.AlbumEntity;
using Domain.Event;

namespace Domain.AlbumAggregate.Events;

public sealed record class AlbumCollaboratorsUpdatedEvent(
    AlbumId Album,
    Collaborators Collaborators
) : IDomainEvent { }
