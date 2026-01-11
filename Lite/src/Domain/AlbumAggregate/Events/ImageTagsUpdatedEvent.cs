using Domain.AlbumAggregate.ImageEntity;
using Domain.Event;

namespace Domain.AlbumAggregate.Events;

public sealed record class ImageTagsUpdatedEvent(ImageId Id, ImageTags Tags) : IDomainEvent { }
