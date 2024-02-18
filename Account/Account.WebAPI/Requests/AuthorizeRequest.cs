using Account.Application.Endpoints.AccountEndpoints.Authorize;
using Account.Domain.UserEntity;
using Account.WebAPI.SeedWorks;

namespace Account.WebAPI.Requests
{
    public readonly struct AuthorizeRequest : ICommandRequestObject<AuthorizeCommand>
    {
        public readonly long UserId { get; init; }
        public readonly Role[] Roles { get; init; }
    }
}
