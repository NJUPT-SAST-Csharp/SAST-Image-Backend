using StackExchange.Redis;

namespace SastImg.Infrastructure.Cache
{
    public class RedisCache : ICache
    {
        private readonly IDatabase _database;

        public RedisCache(IConnectionMultiplexer connectionMultiplexer)
        {
            _database = connectionMultiplexer.GetDatabase();
        }

        public async Task<string?> GetStringAsync(string key)
        {
            return await _database.StringGetAsync(key);
        }

        public Task SetStringAsync(string key, string value, TimeSpan? expiry = null)
        {
            return _database.StringSetAsync(key, value, expiry ?? TimeSpan.FromMinutes(5));
        }
    }
}
