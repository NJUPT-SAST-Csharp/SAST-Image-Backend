namespace Account.Application.Users.Login
{
    public sealed class LoginDto(string jwt)
    {
        public string Jwt { get; } = jwt;
    }
}
