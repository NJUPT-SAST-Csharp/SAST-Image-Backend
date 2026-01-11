using Domain.AlbumAggregate.AlbumEntity;
using Domain.Event;
using Domain.UserAggregate.UserEntity;

namespace Domain.AlbumAggregate.Events;

public sealed record class AlbumSubscribedEvent(AlbumId Album, UserId User) : IDomainEvent { }
