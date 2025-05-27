using Mediator;
using Microsoft.AspNetCore.Http;

namespace Account.Application.Endpoints.AccountEndpoints.Register.VerifyRegistrationCode;

public sealed class VerifyRegistrationCodeCommand(string email, int code) : ICommand<IResult>
{
    public string Email { get; } = email;
    public int Code { get; } = code;
}
