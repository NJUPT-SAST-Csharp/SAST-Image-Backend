namespace Account.Application.Endpoints.AccountEndpoints.ForgetAccount.VerifyForgetCode;

public sealed class VerifyForgetCodeDto(string jwt)
{
    public string Jwt { get; } = jwt;
}
