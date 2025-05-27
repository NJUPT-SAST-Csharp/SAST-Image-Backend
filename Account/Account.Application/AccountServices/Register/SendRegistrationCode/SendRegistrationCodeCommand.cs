using Mediator;

namespace Account.Application.Endpoints.AccountEndpoints.Register.SendRegistrationCode;

public sealed class SendRegistrationCodeCommand(string email) : ICommand
{
    public string Email { get; } = email;
}
