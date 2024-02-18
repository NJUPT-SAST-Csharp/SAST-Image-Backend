using Primitives.Command;

namespace Account.Application.Endpoints.AccountEndpoints.Register.SendRegistrationCode
{
    public sealed class SendRegistrationCodeCommand(string email) : ICommandRequest
    {
        public string Email { get; } = email;
    }
}
