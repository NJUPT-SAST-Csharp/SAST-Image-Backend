using MediatR;
using Primitives.Command;
using Primitives.DomainEvent;
using Primitives.Request;
using Shared.Primitives.DomainEvent;
using Shared.Primitives.Request;

namespace SastImg.Infrastructure.Event
{
    internal class InternalEventBus(IMediator mediator)
        : IQueryRequestSender,
            IDomainEventPublisher,
            ICommandSender
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
            ICommand<TResponse> command,
            CancellationToken cancellationToken = default
        )
        {
            return _mediator.Send(command, cancellationToken);
        }

        public Task CommandAsync(ICommand command, CancellationToken cancellationToken = default)
        {
            return _mediator.Send(command, cancellationToken);
        }
    }
}
