using Domain.Event;
using Domain.UserAggregate.UserEntity;

namespace Domain.UserAggregate.Events;

public sealed record UserRegisteredEvent(UserId Id, Username Username, Nickname Nickname)
    : IDomainEvent;
