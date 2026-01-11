using Domain.Core.Event;
using Domain.Event;
using Mediator;

namespace Infrastructure.Shared.EventBus;

internal sealed class EventPublisher(IMediator publisher) : IDomainEventPublisher
{
    public async Task PublishAsync<TEvent>(
        TEvent domainEvent,
        CancellationToken cancellationToken = default
    )
        where TEvent : IDomainEvent
    {
        await publisher.Publish(domainEvent, cancellationToken);
    }
}
