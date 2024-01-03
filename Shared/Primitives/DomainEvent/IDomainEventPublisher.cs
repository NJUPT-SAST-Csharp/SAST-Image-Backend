using Shared.Primitives.DomainEvent;

namespace Primitives.DomainEvent
{
    public interface IDomainEventPublisher
    {
        public Task PublishAsync<TEvent>(
            TEvent domainEvent,
            CancellationToken cancellationToken = default
        )
            where TEvent : IDomainEvent;
    }
}
