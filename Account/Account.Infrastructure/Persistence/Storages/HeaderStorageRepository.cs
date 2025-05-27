using Account.Application.FileServices;
using Dapper;
using Identity;
using Microsoft.AspNetCore.Http;
using Storage.Clients;

namespace Account.Infrastructure.Persistence.Storages;

internal sealed class HeaderStorageRepository(
    IDbConnectionFactory factory,
    IStorageClient client,
    IProcessClient processor
) : IHeaderStorageRepository
{
    private readonly IDbConnectionFactory _factory = factory;
    private readonly IStorageClient _client = client;
    private readonly IProcessClient _processor = processor;

    public async Task<Stream?> GetHeaderAsync(
        UserId userId,
        CancellationToken cancellationToken = default
    )
    {
        using var connection = _factory.GetConnection();

        const string sql =
            @"
                SELECT header
                FROM users
                WHERE id = @Id";

        var url = await connection.QueryFirstOrDefaultAsync<Uri>(sql, new { Id = userId.Value });

        if (url is null)
            return null;

        return await _client.GetImageAsync(url, cancellationToken);
    }

    public async Task<Uri> UploadHeaderAsync(
        UserId id,
        IFormFile file,
        CancellationToken cancellationToken = default
    )
    {
        string extension = await _processor.GetExtensionNameAsync(file, cancellationToken);

        string key = $"headers/{id}.{extension}";

        await _client.UploadImageAsync(file, key, cancellationToken);

        var url = await _processor.CompressImageAsync(key, true, cancellationToken);

        return url;
    }
}
