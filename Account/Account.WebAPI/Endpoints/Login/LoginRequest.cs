namespace Account.WebAPI.Endpoints.Login
{
    public sealed class LoginRequest
    {
        public string Username { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }
}
