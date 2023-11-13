using Common.Primitives.DomainNotification;
using Common.Primitives.Query;

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
