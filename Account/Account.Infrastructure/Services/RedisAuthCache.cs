using Account.Application.Services;
using StackExchange.Redis;

namespace Account.Infrastructure.Services
{
    internal sealed class RedisAuthCache(IConnectionMultiplexer connection) : IAuthCodeCache
    {
        private readonly IDatabase _database = connection.GetDatabase();

        public Task StoreCodeAsync(
            CodeCacheKey purpose,
            string email,
            int code,
            TimeSpan expiry,
            CancellationToken cancellationToken = default
        )
        {
            return _database.StringSetAsync(
                string.Concat(purpose.ToString(), email.ToUpperInvariant()),
                code,
                expiry
            );
        }

        public Task StoreCodeAsync(
            CodeCacheKey purpose,
            string email,
            int code,
            CancellationToken cancellationToken = default
        )
        {
            return StoreCodeAsync(
                purpose,
                email.ToUpperInvariant(),
                code,
                TimeSpan.FromMinutes(10),
                cancellationToken
            );
        }

        public Task<bool> DeleteCodeAsync(
            CodeCacheKey purpose,
            string email,
            CancellationToken cancellationToken = default
        )
        {
            return _database.KeyDeleteAsync(
                string.Concat(purpose.ToString(), email.ToUpperInvariant())
            );
        }

        public async Task<bool> VerifyCodeAsync(
            CodeCacheKey purpose,
            string email,
            int code,
            CancellationToken cancellationToken = default
        )
        {
            var cacheCode = await _database.StringGetAsync(
                string.Concat(purpose.ToString(), email.ToUpperInvariant())
            );
            if (cacheCode.IsNullOrEmpty)
                return false;
            else
                return cacheCode == code;
        }
    }
}
