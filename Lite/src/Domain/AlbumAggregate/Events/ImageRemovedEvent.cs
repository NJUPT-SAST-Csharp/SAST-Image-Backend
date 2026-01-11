using Domain.AlbumAggregate.ImageEntity;
using Domain.Event;

namespace Domain.AlbumAggregate.Events;

public sealed record class ImageRemovedEvent(ImageId Image, ImageStatus Status) : IDomainEvent { }
