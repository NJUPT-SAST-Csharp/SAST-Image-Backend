using MediatR;
using Primitives.Command;
using Primitives.DomainEvent;
using Primitives.Query;
using Shared.Primitives.DomainEvent;
using Shared.Primitives.Query;

namespace SNS.Infrastructure.EventBus
{
    internal class InternalEventBus(IMediator mediator)
        : ICommandRequestSender,
            IQueryRequestSender,
            IDomainEventPublisher
    {
        private readonly IMediator _mediator = mediator;

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

        public Task PublishAsync<TEvent>(
            TEvent domainEvent,
            CancellationToken cancellationToken = default
        )
            where TEvent : IDomainEvent
        {
            return _mediator.Publish(domainEvent, cancellationToken);
        }

        public Task<TResponse> QueryAsync<TResponse>(
            IQueryRequest<TResponse> request,
            CancellationToken cancellationToken = default
        )
        {
            return _mediator.Send(request, cancellationToken);
        }
    }
}
