using Account.Application.Services;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Account.Infrastructure.Services
{
    internal sealed class RedisAuthCache(
        IConnectionMultiplexer connection,
        ILogger<RedisAuthCache> logger
    ) : IAuthCache
    {
        private readonly ILogger<RedisAuthCache> _logger = logger;
        private readonly IDatabase _database = connection.GetDatabase();

        public Task StoreCodeAsync(
            string email,
            string code,
            TimeSpan expiry,
            CancellationToken cancellationToken = default
        )
        {
            return _database
                .HashSetAsync(CacheKeys.RegistrationCodes, email, code)
                .ContinueWith(async t =>
                {
                    await Task.Delay(expiry);
                    _logger.LogInformation("Registration code {code} has expired.", code);
                    _ = _database.HashDeleteAsync(CacheKeys.RegistrationCodes, email);
                });
        }

        public Task StoreCodeAsync(
            string email,
            string code,
            CancellationToken cancellationToken = default
        )
        {
            return StoreCodeAsync(email, code, TimeSpan.FromMinutes(5), cancellationToken);
        }

        public Task<bool> DeleteCodeAsync(
            string email,
            CancellationToken cancellationToken = default
        )
        {
            return _database.HashDeleteAsync(CacheKeys.RegistrationCodes, email);
        }

        public async Task<bool> VerifyCodeAsync(
            string email,
            string code,
            CancellationToken cancellationToken = default
        )
        {
            var cacheCode = await _database.HashGetAsync(CacheKeys.RegistrationCodes, email);
            if (cacheCode.IsNull)
                return false;
            else
                return cacheCode == code;
        }
    }
}
