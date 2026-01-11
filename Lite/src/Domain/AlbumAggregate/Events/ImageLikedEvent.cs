using Domain.AlbumAggregate.ImageEntity;
using Domain.Event;
using Domain.UserAggregate.UserEntity;

namespace Domain.AlbumAggregate.Events;

public sealed record class ImageLikedEvent(ImageId Image, UserId User) : IDomainEvent { }
