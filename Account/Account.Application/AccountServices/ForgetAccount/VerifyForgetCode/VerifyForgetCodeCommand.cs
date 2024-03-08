using Microsoft.AspNetCore.Http;
using Primitives.Command;

namespace Account.Application.Endpoints.AccountEndpoints.ForgetAccount.VerifyForgetCode
{
    public sealed class VerifyForgetCodeCommand(int code, string email) : ICommandRequest<IResult>
    {
        public string Email { get; } = email;
        public int Code { get; } = code;
    }
}
