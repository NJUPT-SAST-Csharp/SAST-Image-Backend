namespace Account.Application.Endpoints.AccountEndpoints.Login
{
    public sealed class LoginDto(string jwt)
    {
        public string Jwt { get; } = jwt;
    }
}
