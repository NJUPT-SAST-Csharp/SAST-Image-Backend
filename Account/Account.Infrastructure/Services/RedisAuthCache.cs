using Account.Application.Services;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Account.Infrastructure.Services
{
    internal sealed class RedisAuthCache(
        IConnectionMultiplexer connection,
        ILogger<RedisAuthCache> logger
    ) : IAuthCodeCache
    {
        private readonly ILogger<RedisAuthCache> _logger = logger;
        private readonly IDatabase _database = connection.GetDatabase();

        public Task StoreCodeAsync(
            CodeCaheKey purpose,
            string email,
            int code,
            TimeSpan expiry,
            CancellationToken cancellationToken = default
        )
        {
            return _database
                .HashSetAsync(purpose.ToString(), email.ToUpperInvariant(), code)
                .ContinueWith(async t =>
                {
                    await Task.Delay(expiry);
                    _logger.LogInformation("Registration code {code} has expired.", code);
                    _ = _database.HashDeleteAsync(purpose.ToString(), email);
                });
        }

        public Task StoreCodeAsync(
            CodeCaheKey purpose,
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
            CodeCaheKey purpose,
            string email,
            CancellationToken cancellationToken = default
        )
        {
            return _database.HashDeleteAsync(purpose.ToString(), email.ToUpperInvariant());
        }

        public async Task<bool> VerifyCodeAsync(
            CodeCaheKey purpose,
            string email,
            int code,
            CancellationToken cancellationToken = default
        )
        {
            var cacheCode = await _database.HashGetAsync(
                purpose.ToString(),
                email.ToUpperInvariant()
            );
            if (cacheCode.IsNull)
                return false;
            else
                return cacheCode == code;
        }
    }
}
