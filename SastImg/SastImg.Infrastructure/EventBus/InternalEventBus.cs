using MediatR;
using SastImg.Application.Services.EventBus;
using Shared.Primitives.DomainNotification;
using Shared.Primitives.Query;

namespace SastImg.Infrastructure.Event
{
    internal class InternalEventBus : IInternalEventBus
    {
        private readonly IMediator _mediator;

        public InternalEventBus(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task PublishAsync<TNotification>(
            TNotification notification,
            CancellationToken cancellationToken = default
        )
            where TNotification : IDomainNotification
        {
            return _mediator.Publish(notification, cancellationToken);
        }

        public Task<TResponse> RequestAsync<TResponse>(
            IQuery<TResponse> query,
            CancellationToken cancellationToken = default
        )
        {
            return _mediator.Send(query, cancellationToken);
        }
    }
}
