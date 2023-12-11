namespace Account.Application.Endpoints.AccountEndpoints.Register.VerifyRegistrationCode
{
    public sealed class VerifyRegistrationCodeDto(string jwt)
    {
        public string Jwt { get; } = jwt;
    }
}
