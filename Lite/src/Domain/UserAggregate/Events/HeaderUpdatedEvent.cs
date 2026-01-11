using Domain.Event;
using Domain.Shared;
using Domain.UserAggregate.UserEntity;

namespace Domain.UserAggregate.Events;

public sealed record HeaderUpdatedEvent(UserId User, IImageFile Header) : IDomainEvent { }
