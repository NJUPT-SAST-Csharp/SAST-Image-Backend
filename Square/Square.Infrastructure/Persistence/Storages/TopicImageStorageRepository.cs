using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.ObjectPool;
using Square.Application.TopicServices;
using Storage.Clients;

namespace Square.Infrastructure.Persistence.Storages
{
    internal class TopicImageStorageRepository(
        IStorageClient storage,
        IProcessClient processor,
        ObjectPool<StringBuilder> builderPool
    ) : ITopicImageStorageRepository
    {
        private readonly IStorageClient _storage = storage;
        private readonly IProcessClient _processor = processor;

        public async Task<(Uri, Uri)> UploadImageAsync(
            IFormFile file,
            CancellationToken cancellationToken = default
        )
        {
            await using var image = file.OpenReadStream();

            var extension = await _processor.GetExtensionNameAsync(image, cancellationToken);

            var builder = builderPool.Get();

            builder
                .Append("topic-images")
                .Append('/')
                .Append(DateTime.UtcNow.ToString("yyyy/MM/dd"))
                .Append('/')
                .Append(Guid.NewGuid().ToString("N"))
                .Append('.')
                .Append(extension);

            string mainKey = builder.ToString();

            builderPool.Return(builder);

            var imageUrl = await _storage.UploadImageAsync(image, mainKey, cancellationToken);

            var compressedImageUrl = await _processor.ProcessImageAsync(mainKey, cancellationToken);

            return (imageUrl, compressedImageUrl);
        }
    }
}
