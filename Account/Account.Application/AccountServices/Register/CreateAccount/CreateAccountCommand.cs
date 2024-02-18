using Primitives.Command;

namespace Account.Application.Endpoints.AccountEndpoints.Register.CreateAccount
{
    public sealed class CreateAccountCommand(
        string username,
        string password,
        string email,
        int code
    ) : ICommandRequest<CreateAccountDto>
    {
        public string Username { get; init; } = username;
        public string Password { get; init; } = password;
        public string Email { get; init; } = email;
        public int Code { get; init; } = code;
    }
}
