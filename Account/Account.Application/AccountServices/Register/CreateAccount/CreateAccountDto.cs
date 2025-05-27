namespace Account.Application.Endpoints.AccountEndpoints.Register.CreateAccount;

public sealed class CreateAccountDto(string jwt)
{
    public string Jwt { get; } = jwt;
}
