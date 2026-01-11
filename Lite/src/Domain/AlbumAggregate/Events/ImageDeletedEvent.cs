using Domain.AlbumAggregate.ImageEntity;
using Domain.Event;

namespace Domain.AlbumAggregate.Events;

public sealed record ImageDeletedEvent(ImageId Image) : IDomainEvent { }
