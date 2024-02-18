using Primitives.Command;

namespace Account.Application.Endpoints.AccountEndpoints.ForgetAccount.SendForgetCode
{
    public sealed class SendForgetCodeCommand(string email) : ICommandRequest
    {
        public string Email { get; } = email;
    }
}
