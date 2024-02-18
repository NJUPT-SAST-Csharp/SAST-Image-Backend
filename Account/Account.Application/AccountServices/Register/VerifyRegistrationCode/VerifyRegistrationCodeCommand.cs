using Primitives.Command;

namespace Account.Application.Endpoints.AccountEndpoints.Register.VerifyRegistrationCode
{
    public sealed class VerifyRegistrationCodeCommand : ICommandRequest<bool>
    {
        public string Email { get; }
        public int Code { get; }
    }
}
