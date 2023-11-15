using System.Text.Json;
using SastImg.Application.Services;
using StackExchange.Redis;

namespace SastImg.Infrastructure.Cache
{
    internal class RedisCache(IConnectionMultiplexer connectionMultiplexer) : ICache
    {
        private readonly IDatabase _database = connectionMultiplexer.GetDatabase();

        public async Task<string?> GetStringAsync(string key)
        {
            return await _database.StringGetAsync(key);
        }

        public async Task<TValue?> HashGetAsync<TValue>(string key, string field)
        {
            var json = await _database.HashGetAsync(key, field);
            if (json.IsNull)
                return default;
            var result = JsonSerializer.Deserialize<TValue>(json.ToString());
            return result;
        }

        public Task<bool> HashSetAsync<TValue>(string key, string field, TValue value)
        {
            var json = JsonSerializer.Serialize(value);
            return _database.HashSetAsync(key, field, json);
        }

        public Task<bool> SetStringAsync(string key, string value, TimeSpan? expiry = null)
        {
            return _database.StringSetAsync(key, value, expiry ?? TimeSpan.FromMinutes(5));
        }
    }
}
