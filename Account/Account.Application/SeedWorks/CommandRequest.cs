using System.Security.Claims;
using Primitives.Command;

namespace Account.Application.SeedWorks
{
    public sealed class CommandRequest<TRequest, TResponse>(
        in TRequest request,
        in ClaimsPrincipal user
    ) : ICommandRequest<TResponse>
    {
        public TRequest Request { get; } = request;
        public RequesterInfo Requester { get; } = new(user);
    }

    public sealed class CommandRequest<TRequest>(in TRequest request, in ClaimsPrincipal user)
        : ICommandRequest
    {
        public TRequest Request { get; } = request;
        public RequesterInfo Requester { get; } = new(user);
    }
}
