using Mediator;

namespace Account.Application.Endpoints.AccountEndpoints.ForgetAccount.SendForgetCode;

public sealed class SendForgetCodeCommand(string email) : ICommand
{
    public string Email { get; } = email;
}
