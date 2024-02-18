using Account.Application.Endpoints.AccountEndpoints.Authorize;
using Account.WebAPI.SeedWorks;

namespace Account.WebAPI.Requests
{
    public readonly struct AuthorizeRequest : IRequestObject<AuthorizeCommand>
    {
        public readonly long UserId { get; }
        public readonly int RoleId { get; }
    }
}
