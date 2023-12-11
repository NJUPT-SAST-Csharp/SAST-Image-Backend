using Account.Application.SeedWorks;

namespace Account.Application.Endpoints.AccountEndpoints.Register.CreateAccount
{
    public sealed class CreateAccountRequest : IRequest
    {
        public required string Username { get; init; }
        public required string Password { get; init; }
    }
}
