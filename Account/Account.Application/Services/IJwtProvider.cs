namespace Account.Application.Services
{
    public interface IJwtProvider
    {
        public string GetRegistrantJwt(string email);

        public string GetLoginJwt(string userId, string username);
    }
}
