using Domain.AlbumAggregate.ImageEntity;
using Domain.Event;

namespace Domain.AlbumAggregate.Events;

public sealed record class ImageRestoredEvent(ImageId Image, ImageStatus Status) : IDomainEvent { }
