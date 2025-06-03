using Storage.Application.Model;
using Storage.Application.Service;

namespace Storage.Infrastructure.Service;

internal sealed class TokenRepository(StackExchange.Redis.IConnectionMultiplexer connection)
    : ITokenRepository
{
    private const string HashSetName = "FileTokens";
    private static readonly TimeSpan TokenExpiration = TimeSpan.FromMinutes(1);

    public async Task<bool> ConfirmAsync(
        FileToken token,
        CancellationToken cancellationToken = default
    )
    {
        var database = connection.GetDatabase();
        var value = await database.HashGetAsync(HashSetName, token.Value);
        if (value.HasValue is false || value.TryParse(out long timestamp) is false)
        {
            return false;
        }
        await database.HashDeleteAsync(HashSetName, token.Value);
        var expireAt = DateTimeOffset.FromUnixTimeSeconds(timestamp).UtcDateTime;
        return expireAt < DateTime.UtcNow;
    }

    public Task InsertAsync(FileToken token, CancellationToken cancellationToken = default)
    {
        var database = connection.GetDatabase();
        var expireAt = DateTimeOffset.UtcNow + TokenExpiration;
        long timestamp = expireAt.ToUnixTimeSeconds();

        return database.HashSetAsync(
            HashSetName,
            token.Value,
            timestamp,
            when: StackExchange.Redis.When.NotExists,
            flags: StackExchange.Redis.CommandFlags.FireAndForget
        );
    }
}
