using Shared.Primitives.DomainEvent;

namespace Primitives.DomainEvent
{
    public interface IDomainEventPublisher
    {
        public Task PublishAsync<TEvent>(
            TEvent @event,
            CancellationToken cancellationToken = default
        )
            where TEvent : IDomainEvent;
    }
}
