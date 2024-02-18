using MediatR;
using Primitives.Command;
using Primitives.DomainEvent;
using Primitives.Query;
using Shared.Primitives.DomainEvent;
using Shared.Primitives.Query;

namespace Account.Infrastructure.EventBus
{
    internal class InternalEventBus(IMediator mediator)
        : IQueryRequestSender,
            IDomainEventPublisher,
            ICommandRequestSender
    {
        private readonly IMediator _mediator = mediator;

        public Task PublishAsync<TEvent>(
            TEvent @event,
            CancellationToken cancellationToken = default
        )
            where TEvent : IDomainEvent
        {
            return _mediator.Publish(@event, cancellationToken);
        }

        public Task<TResponse> QueryAsync<TResponse>(
            IQueryRequest<TResponse> request,
            CancellationToken cancellationToken = default
        )
        {
            return _mediator.Send(request, cancellationToken);
        }

        public Task<TResponse> CommandAsync<TResponse>(
            ICommandRequest<TResponse> command,
            CancellationToken cancellationToken = default
        )
        {
            return _mediator.Send(command, cancellationToken);
        }

        public Task CommandAsync(
            ICommandRequest command,
            CancellationToken cancellationToken = default
        )
        {
            return _mediator.Send(command, cancellationToken);
        }
    }
}
