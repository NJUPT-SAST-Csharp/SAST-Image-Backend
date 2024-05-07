using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.ObjectPool;
using SastImg.Application.ImageServices.AddImage;
using Storage.Clients;

namespace SastImg.Infrastructure.Persistence.Storages
{
    internal sealed class ImageStorageRepository(
        IStorageClient client,
        IProcessClient processor,
        ObjectPool<StringBuilder> builderPool
    ) : IImageStorageRepository
    {
        private readonly IStorageClient _client = client;
        private readonly IProcessClient _processor = processor;
        private readonly ObjectPool<StringBuilder> _builderPool = builderPool;

        public async Task<(Uri, Uri)> UploadImageAsync(
            IFormFile file,
            CancellationToken cancellationToken = default
        )
        {
            await using var image = file.OpenReadStream();

            var extension = await _processor.GetExtensionNameAsync(image, cancellationToken);

            var builder = _builderPool.Get();

            var key = builder
                .Append("images")
                .Append('/')
                .Append(DateTime.UtcNow.ToString("yyyy/MM/dd"))
                .Append('/')
                .Append(Guid.NewGuid().ToString("N"))
                .Append('.')
                .Append(extension)
                .ToString();

            _builderPool.Return(builder);

            var url = await _client.UploadImageAsync(image, key, cancellationToken);

            var compressedUrl = await _processor.CompressImageAsync(key, cancellationToken);

            return (url, compressedUrl);
        }
    }
}
