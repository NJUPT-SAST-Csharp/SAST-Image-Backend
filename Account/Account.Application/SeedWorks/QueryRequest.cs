using System.Security.Claims;
using Shared.Primitives.Query;

namespace Account.Application.SeedWorks
{
    public sealed class QueryRequest<TRequest, TResponse>(
        in TRequest request,
        in ClaimsPrincipal user
    ) : IQueryRequest<TResponse>
    {
        public TRequest Request { get; } = request;
        public RequesterInfo Requester { get; } = new(user);
    }
}
