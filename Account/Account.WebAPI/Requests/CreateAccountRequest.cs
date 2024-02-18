using Account.Application.Endpoints.AccountEndpoints.Register.CreateAccount;
using Account.WebAPI.SeedWorks;

namespace Account.WebAPI.Requests
{
    public readonly struct CreateAccountRequest
        : ICommandRequestObject<CreateAccountCommand, CreateAccountDto>
    {
        public readonly string Username { get; init; }
        public readonly string Password { get; init; }
        public readonly string Email { get; init; }
        public readonly int Code { get; init; }
    }
}
