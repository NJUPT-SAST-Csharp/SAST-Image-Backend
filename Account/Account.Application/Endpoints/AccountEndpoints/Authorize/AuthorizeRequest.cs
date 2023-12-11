using Account.Application.SeedWorks;

namespace Account.Application.Endpoints.AccountEndpoints.Authorize
{
    public sealed class AuthorizeRequest : IRequest
    {
        public required long UserId { get; init; }
        public required int RoleId { get; init; }
    }
}
