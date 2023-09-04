using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace SastImgAPI.Services
{
    public class CacheAuthAccessor
    {
        private readonly IDistributedCache _cache;
        private readonly DistributedCacheEntryOptions _options;

        public CacheAuthAccessor(IDistributedCache cache)
        {
            _cache = cache;
            _options = new()
            {
                SlidingExpiration = TimeSpan.FromMinutes(10),
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            };
        }

        public async Task SetAuthAsync(
            string username,
            string newAuthValue,
            CancellationToken clt = default
        )
        {
            string key = "Auth_" + username;
            var value = await _cache.GetStringAsync(key);
            if (value is null)
            {
                var json = JsonSerializer.Serialize(new HashSet<string> { newAuthValue });
                await _cache.SetStringAsync(key, json, _options, clt);
            }
            else
            {
                var set = JsonSerializer.Deserialize<HashSet<string>>(value)!;
                set.Add(value);
                var json = JsonSerializer.Serialize(set);
                await _cache.SetStringAsync(key, json, _options, clt);
            }
        }

        public async Task<bool> IsInAuthAsync(
            string username,
            string value,
            CancellationToken clt = default
        )
        {
            string key = "Auth_" + username;
            var str = await _cache.GetStringAsync(key, clt);
            if (str is null)
                return false;
            else
            {
                var set = JsonSerializer.Deserialize<IReadOnlyCollection<string>>(value)!;
                return set.Contains(str);
            }
        }

        public async Task<IReadOnlySet<string>?> GetAuthSetAsync(
            string username,
            CancellationToken clt = default
        )
        {
            string key = "Auth_" + username;
            var value = await _cache.GetStringAsync(key, clt);
            if (value is null)
                return null;
            else
            {
                var set = JsonSerializer.Deserialize<HashSet<string>>(value)!;
                return set;
            }
        }
    }
}
