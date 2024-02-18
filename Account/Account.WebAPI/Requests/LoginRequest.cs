using Account.Application.Endpoints.AccountEndpoints.Login;
using Account.WebAPI.SeedWorks;

namespace Account.WebAPI.Requests
{
    public readonly struct LoginRequest : ICommandRequestObject<LoginCommand, LoginDto>
    {
        public readonly string Username { get; init; }
        public readonly string Password { get; init; }
    }
}
