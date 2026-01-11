using Domain.Event;
using Domain.UserAggregate.UserEntity;

namespace Domain.UserAggregate.Events;

public sealed record BiographyUpdatedEvent(UserId User, Biography Biography) : IDomainEvent { }
