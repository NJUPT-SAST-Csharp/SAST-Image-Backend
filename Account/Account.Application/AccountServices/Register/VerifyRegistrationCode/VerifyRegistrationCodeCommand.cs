using Account.Application.AccountServices.Register.VerifyRegistrationCode;
using Primitives.Command;

namespace Account.Application.Endpoints.AccountEndpoints.Register.VerifyRegistrationCode
{
    public sealed class VerifyRegistrationCodeCommand(string email, int code)
        : ICommandRequest<VerifyRegistrationCodeDto>
    {
        public string Email { get; } = email;
        public int Code { get; } = code;
    }
}
