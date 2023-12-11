using Account.Application.SeedWorks;

namespace Account.Application.Endpoints.AccountEndpoints.Login
{
    public sealed class LoginRequest : IRequest
    {
        public required string Username { get; init; }
        public required string Password { get; init; }
    }
}
