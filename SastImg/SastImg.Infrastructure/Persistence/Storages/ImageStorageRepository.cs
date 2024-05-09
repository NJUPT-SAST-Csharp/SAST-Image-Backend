using System.Text;
using Dapper;
using Microsoft.AspNetCore.Http;
using SastImg.Application.ImageServices.AddImage;
using SastImg.Domain.AlbumAggregate.ImageEntity;
using SastImg.Infrastructure.Persistence.QueryDatabase;
using Storage.Clients;

namespace SastImg.Infrastructure.Persistence.Storages
{
    internal sealed class ImageStorageRepository(
        IDbConnectionFactory factory,
        IStorageClient client,
        IProcessClient processor
    ) : IImageStorageRepository
    {
        private readonly IDbConnectionFactory _factory = factory;

        private readonly IStorageClient _client = client;
        private readonly IProcessClient _processor = processor;

        public async Task<Stream?> GetImageAsync(
            ImageId imageId,
            bool isThumbnail = false,
            CancellationToken cancellationToken = default
        )
        {
            using var conncection = _factory.GetConnection();

            const string sql =
                @"
                SELECT i.url AS Original,
                i.thumbnail AS Thumbnail
                FROM images AS i
                WHERE id = @Id";

            ImageUrl? url = await conncection.QueryFirstOrDefaultAsync<ImageUrl?>(
                sql,
                new { Id = imageId.Value }
            );

            if (url is null)
                return null;

            return await _client.GetImageAsync(
                isThumbnail ? url.Thumbnail : url.Original,
                cancellationToken
            );
        }

        public async Task<ImageUrl> UploadImageAsync(
            IFormFile file,
            CancellationToken cancellationToken = default
        )
        {
            var extension = await _processor.GetExtensionNameAsync(file, cancellationToken);

            var key = new StringBuilder(64)
                .Append("images")
                .Append('/')
                .Append(DateTime.UtcNow.ToString("yyyy/MM/dd"))
                .Append('/')
                .Append(Guid.NewGuid().ToString("N"))
                .Append('.')
                .Append(extension)
                .ToString();

            var url = await _client.UploadImageAsync(file, key, cancellationToken);

            var compressedUrl = await _processor.CompressImageAsync(key, false, cancellationToken);

            return new(url, compressedUrl);
        }
    }
}
