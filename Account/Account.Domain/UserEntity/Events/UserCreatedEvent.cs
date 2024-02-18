using Shared.Primitives.DomainEvent;

namespace Account.Domain.UserEntity.Events
{
    public sealed class UserCreatedEvent(User user) : IDomainEvent
    {
        public UserId UserId { get; } = user.Id;
    }
}
