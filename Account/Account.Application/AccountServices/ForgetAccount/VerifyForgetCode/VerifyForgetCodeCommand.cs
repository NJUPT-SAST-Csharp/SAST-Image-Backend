using Mediator;
using Microsoft.AspNetCore.Http;

namespace Account.Application.Endpoints.AccountEndpoints.ForgetAccount.VerifyForgetCode;

public sealed class VerifyForgetCodeCommand(int code, string email) : ICommand<IResult>
{
    public string Email { get; } = email;
    public int Code { get; } = code;
}
