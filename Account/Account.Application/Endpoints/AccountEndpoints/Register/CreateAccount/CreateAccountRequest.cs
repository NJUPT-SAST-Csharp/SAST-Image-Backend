using Account.Application.SeedWorks;

namespace Account.Application.Endpoints.AccountEndpoints.Register.CreateAccount
{
    public sealed class CreateAccountRequest : IRequest
    {
        public string Username { get; init; }
        public string Password { get; init; }
        public string Email { get; init; }
        public int Code { get; init; }
    }
}
