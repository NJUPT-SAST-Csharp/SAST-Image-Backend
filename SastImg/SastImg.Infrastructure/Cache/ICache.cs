namespace SastImg.Infrastructure.Cache
{
    public interface ICache
    {
        public Task SetStringAsync(string key, string value, TimeSpan? expiry = null);
        public Task<string?> GetStringAsync(string key);
    }
}
