using Domain.Event;
using Domain.UserAggregate.UserEntity;

namespace Domain.UserAggregate.Events;

public sealed record UsernameResetEvent(UserId UserId, Username Username) : IDomainEvent;
