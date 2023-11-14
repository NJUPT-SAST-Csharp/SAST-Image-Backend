using Shared.Primitives.DomainNotification;
using Shared.Primitives.Query;

namespace SastImg.Application.Services.EventBus
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
