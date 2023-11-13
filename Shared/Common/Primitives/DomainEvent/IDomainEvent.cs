using Shared.DomainPrimitives.DomainNotification;

namespace Shared.DomainPrimitives.DomainEvent
{
    public interface IDomainEvent : IDomainNotification
    {
        public DateTime RegisteredAt { get; }
    }
}
