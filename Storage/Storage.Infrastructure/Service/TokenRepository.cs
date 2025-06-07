using System.Runtime.CompilerServices;
using StackExchange.Redis;
using Storage.Application.Model;
using Storage.Application.Service;
using Storage.Infrastructure.Models;

namespace Storage.Infrastructure.Service;

internal sealed class TokenRepository(IConnectionMultiplexer connection) : ITokenRepository
{
    private const string HashSetName = "FileTokens";

    public async Task<bool> ExistsAsync(
        IFileToken token,
        CancellationToken cancellationToken = default
    )
    {
        var database = connection.GetDatabase();
        var value = await database
            .HashGetAsync(HashSetName, token.Value)
            .WaitAsync(cancellationToken);

        return value.HasValue;
    }

    public async Task<IFileToken[]> GetExpiredAsync(CancellationToken cancellationToken = default)
    {
        var database = connection.GetDatabase();
        var values = await database.HashGetAllAsync(HashSetName).WaitAsync(cancellationToken);
        return values
            .Where(e =>
                e.Value.TryParse(out long timestamp)
                && DateTimeOffset.FromUnixTimeSeconds(timestamp).UtcDateTime < DateTime.UtcNow
            )
            .Select(e => Create(e.Name.ToString()))
            .ToArray();
    }

    public Task DeleteAsync(IFileToken token, CancellationToken cancellationToken = default)
    {
        var database = connection.GetDatabase();
        return database
            .HashDeleteAsync(HashSetName, token.Value, CommandFlags.FireAndForget)
            .WaitAsync(cancellationToken);
    }

    public Task DeleteAsync(IFileToken[] tokens, CancellationToken cancellationToken = default)
    {
        var database = connection.GetDatabase();
        return database
            .HashDeleteAsync(
                HashSetName,
                tokens.Select(t => new RedisValue(t.Value)).ToArray(),
                CommandFlags.FireAndForget
            )
            .WaitAsync(cancellationToken);
    }

    public Task AddAsync(IFileToken token, CancellationToken cancellationToken = default)
    {
        var database = connection.GetDatabase();

        return database.HashSetAsync(
            HashSetName,
            token.Value,
            token.ExpireAt.ToBinary(),
            when: When.NotExists,
            flags: CommandFlags.FireAndForget
        );
    }

    [UnsafeAccessor(UnsafeAccessorKind.Constructor)]
    private static extern FileToken Create(string base64String);
}
