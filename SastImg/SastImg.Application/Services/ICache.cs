namespace SastImg.Application.Services
{
    public interface ICache
    {
        public Task<bool> StringSetAsync(string key, string value, TimeSpan? expiry = null);
        public Task<string?> StringGetAsync(string key);

        public Task<bool> HashSetAsync<TValue>(string key, long field, TValue value)
            where TValue : class;
        public Task<bool> HashSetAsync<TValue>(string key, string field, TValue value)
            where TValue : class;
        public Task HashSetAsync<TValue>(string key, IEnumerable<(long, TValue)> values)
            where TValue : class;

        public Task<TValue?> HashGetAsync<TValue>(string key, long field)
            where TValue : class;
        public Task<TValue?> HashGetAsync<TValue>(string key, string field)
            where TValue : class;
        public Task<IEnumerable<TValue>> HashGetAsync<TValue>(string key)
            where TValue : class;
    }
}
