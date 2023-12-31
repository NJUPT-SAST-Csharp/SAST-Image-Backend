using Shared.Primitives.DomainEvent;

namespace Primitives.DomainEvent
{
    public abstract class DomainEventBase : IDomainEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }
}
