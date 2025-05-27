using Mediator;
using Microsoft.AspNetCore.Http;

namespace Account.Application.Endpoints.AccountEndpoints.Register.CreateAccount;

public sealed class CreateAccountCommand(string username, string password, string email, int code)
    : ICommand<IResult>
{
    public string Username { get; init; } = username;
    public string Password { get; init; } = password;
    public string Email { get; init; } = email;
    public int Code { get; init; } = code;
}
