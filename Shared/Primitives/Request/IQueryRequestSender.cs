using Shared.Primitives.Request;

namespace Primitives.Request
{
    public interface IQueryRequestSender
    {
        public Task<TResponse> RequestAsync<TResponse>(
            IQueryRequest<TResponse> request,
            CancellationToken cancellationToken = default
        );
    }
}
