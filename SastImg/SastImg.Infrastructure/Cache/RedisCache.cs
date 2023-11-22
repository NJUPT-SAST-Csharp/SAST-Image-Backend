using System.Text.Json;
using SastImg.Application.Services;
using StackExchange.Redis;

namespace SastImg.Infrastructure.Cache
{
    internal class RedisCache(IConnectionMultiplexer connectionMultiplexer) : ICache
    {
        private readonly IDatabase _database = connectionMultiplexer.GetDatabase();

        public async Task<string?> StringGetAsync(string key)
        {
            return await _database.StringGetAsync(key);
        }

        public async Task<TValue?> HashGetAsync<TValue>(string key, long field)
            where TValue : class
        {
            var json = await _database.HashGetAsync(key, field);
            if (json.IsNull)
                return default;
            var result = JsonSerializer.Deserialize<TValue>(json.ToString());
            return result;
        }

        public Task<bool> HashSetAsync<TValue>(string key, long field, TValue value)
            where TValue : class
        {
            var json = JsonSerializer.Serialize(value);
            return _database.HashSetAsync(key, field, json);
        }

        public Task<bool> StringSetAsync(string key, string value, TimeSpan? expiry = null)
        {
            return _database.StringSetAsync(key, value, expiry ?? TimeSpan.FromMinutes(10));
        }

        public Task HashSetAsync<TValue>(string key, IEnumerable<(long, TValue)> values)
            where TValue : class
        {
            var entries = values
                .Select(v => new HashEntry(v.Item1, JsonSerializer.Serialize(v.Item2)))
                .ToArray();
            return _database.HashSetAsync(key, entries);
        }

        public async Task<IEnumerable<TValue>> HashGetAsync<TValue>(string key)
            where TValue : class
        {
            var data = await _database.HashGetAllAsync(key);
            return data.Select(v => JsonSerializer.Deserialize<TValue>(v.Value.ToString())!);
        }

        public Task<bool> HashSetAsync<TValue>(string key, string field, TValue value)
            where TValue : class
        {
            var json = JsonSerializer.Serialize(value);
            return _database.HashSetAsync(key, field, json);
        }

        public async Task<TValue?> HashGetAsync<TValue>(string key, string field)
            where TValue : class
        {
            var json = await _database.HashGetAsync(key, field);
            if (json.IsNull)
                return default;
            var result = JsonSerializer.Deserialize<TValue>(json.ToString());
            return result;
        }
    }
}
