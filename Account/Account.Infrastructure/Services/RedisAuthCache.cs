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
            string purpose,
            string email,
            int code,
            TimeSpan expiry,
            CancellationToken cancellationToken = default
        )
        {
            return _database
                .HashSetAsync(purpose, email.ToUpperInvariant(), code)
                .ContinueWith(async t =>
                {
                    await Task.Delay(expiry);
                    _logger.LogInformation("Registration code {code} has expired.", code);
                    _ = _database.HashDeleteAsync(purpose, email);
                });
        }

        public Task StoreCodeAsync(
            string purpose,
            string email,
            int code,
            CancellationToken cancellationToken = default
        )
        {
            return StoreCodeAsync(
                purpose,
                email.ToUpperInvariant(),
                code,
                TimeSpan.FromMinutes(5),
                cancellationToken
            );
        }

        public Task<bool> DeleteCodeAsync(
            string purpose,
            string email,
            CancellationToken cancellationToken = default
        )
        {
            return _database.HashDeleteAsync(purpose, email.ToUpperInvariant());
        }

        public async Task<bool> VerifyCodeAsync(
            string purpose,
            string email,
            int code,
            CancellationToken cancellationToken = default
        )
        {
            var cacheCode = await _database.HashGetAsync(purpose, email.ToUpperInvariant());
            if (cacheCode.IsNull)
                return false;
            else
                return cacheCode == code;
        }
    }
}
