using Common.Primitives.DomainNotification;

namespace Common.Primitives.DomainEvent
{
    public interface IDomainEvent : IDomainNotification
    {
        public DateTime RegisteredAt { get; }
    }
}
