using Account.Application.Endpoints.AccountEndpoints.Register.CreateAccount;

namespace Account.WebAPI.Requests
{
    public readonly struct CreateAccountRequest
    {
        public readonly string Username { get; init; }
        public readonly string Password { get; init; }
        public readonly string Email { get; init; }
        public readonly int Code { get; init; }

        public CreateAccountCommand ToCommand()
        {
            return new CreateAccountCommand(Username, Password, Email, Code);
        }
    }
}
