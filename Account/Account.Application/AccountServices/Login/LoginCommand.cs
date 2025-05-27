using Mediator;
using Microsoft.AspNetCore.Http;

namespace Account.Application.Endpoints.AccountEndpoints.Login;

public sealed class LoginCommand(string username, string password) : ICommand<IResult>
{
    public string Username { get; } = username;
    public string Password { get; } = password;
}
