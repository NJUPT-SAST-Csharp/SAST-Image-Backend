using Domain.AlbumAggregate.ImageEntity;
using Domain.Event;
using Domain.UserAggregate.UserEntity;

namespace Domain.AlbumAggregate.Events;

public sealed record class ImageUnlikedEvent(ImageId Image, UserId User) : IDomainEvent { }
