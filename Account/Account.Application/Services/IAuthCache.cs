namespace Account.Application.Services
{
    public interface IAuthCache
    {
        public Task StoreCodeAsync(string key, string code, TimeSpan expiry);

        public Task StoreCodeAsync(string key, string code);

        public Task<bool> VerifyCodeAsync(string key, string code);
    }
}
