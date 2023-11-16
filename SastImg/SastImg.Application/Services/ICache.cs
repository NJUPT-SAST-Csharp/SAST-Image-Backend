namespace SastImg.Application.Services
{
    public interface ICache
    {
        public Task<bool> StringSetAsync(string key, string value, TimeSpan? expiry = null);
        public Task<string?> StringGetAsync(string key);
        public Task<bool> HashSetAsync<TValue>(string key, long field, TValue value);
        public Task HashSetAsync<TValue>(string key, IEnumerable<(long, TValue)> values)
            where TValue : class;
        public Task<TValue?> HashGetAsync<TValue>(string key, long field);
        public Task<IEnumerable<TValue>> HashGetAsync<TValue>(string key)
            where TValue : class;
    }
}
