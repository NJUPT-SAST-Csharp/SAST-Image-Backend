using Common.Primitives.Query;
using Shared.DomainPrimitives.DomainNotification;

namespace SastImg.Application.EventBus
{
    public interface IInternalEventBus
    {
        public Task<TResponse> RequestAsync<TResponse>(
            IQuery<TResponse> query,
            CancellationToken cancellationToken = default
        );

        public Task PublishAsync<TNotification>(
            TNotification notification,
            CancellationToken cancellationToken = default
        )
            where TNotification : IDomainNotification;
    }
}
