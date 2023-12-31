using Shared.Primitives.Request;

namespace Primitives.Request
{
    public interface IQueryRequestSender
    {
        public Task<TResponse> QueryAsync<TResponse>(
            IQueryRequest<TResponse> request,
            CancellationToken cancellationToken = default
        );
    }
}
