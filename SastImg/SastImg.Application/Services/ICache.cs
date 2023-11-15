namespace SastImg.Application.Services
{
    public interface ICache
    {
        public Task<bool> SetStringAsync(string key, string value, TimeSpan? expiry = null);
        public Task<string?> GetStringAsync(string key);
        public Task<bool> HashSetAsync<TValue>(string key, string field, TValue value);
        public Task<TValue?> HashGetAsync<TValue>(string key, string field);
    }
}
