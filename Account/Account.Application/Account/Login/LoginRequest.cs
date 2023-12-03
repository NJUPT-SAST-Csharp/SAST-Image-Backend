using Account.Application.SeedWorks;

namespace Account.Application.Account.Login
{
    public sealed class LoginRequest : IRequest
    {
        public string Username { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }
}
