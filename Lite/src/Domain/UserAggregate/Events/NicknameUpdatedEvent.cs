using Domain.Event;
using Domain.UserAggregate.UserEntity;

namespace Domain.UserAggregate.Events;

public sealed record class NicknameUpdatedEvent(UserId Id, Nickname Nickname) : IDomainEvent { }
