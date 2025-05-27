using Account.Application.FileServices;
using Dapper;
using Identity;
using Microsoft.AspNetCore.Http;
using Storage.Clients;

namespace Account.Infrastructure.Persistence.Storages;

internal sealed class AvatarStorageRepository(
    IDbConnectionFactory factory,
    IStorageClient client,
    IProcessClient processor
) : IAvatarStorageRepository
{
    private readonly IDbConnectionFactory _factory = factory;
    private readonly IStorageClient _client = client;
    private readonly IProcessClient _processor = processor;

    public async Task<Stream?> GetAvatarAsync(
        UserId userId,
        CancellationToken cancellationToken = default
    )
    {
        using var connection = _factory.GetConnection();

        const string sql =
            @"
                SELECT avatar
                FROM users
                WHERE id = @Id";

        var url = await connection.QueryFirstOrDefaultAsync<Uri>(sql, new { Id = userId.Value });

        if (url is null)
            return null;

        return await _client.GetImageAsync(url, cancellationToken);
    }

    public async Task<Uri> UploadAvatarAsync(
        UserId id,
        IFormFile avatarFile,
        CancellationToken cancellationToken = default
    )
    {
        string extension = await _processor.GetExtensionNameAsync(avatarFile, cancellationToken);

        string key = $"avatars/{id}.{extension}";

        var url = await _client.UploadImageAsync(avatarFile, key, cancellationToken);

        return url;
    }
}
