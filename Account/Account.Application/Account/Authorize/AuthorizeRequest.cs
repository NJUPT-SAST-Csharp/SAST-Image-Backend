using Account.Application.SeedWorks;

namespace Account.Application.Account.Authorize
{
    public sealed class AuthorizeRequest : IRequest
    {
        public required long UserId { get; init; }
        public required int RoleId { get; init; }
    }
}
