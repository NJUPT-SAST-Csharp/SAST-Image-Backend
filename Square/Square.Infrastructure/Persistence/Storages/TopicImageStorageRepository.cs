using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.ObjectPool;
using Square.Application.TopicServices;
using Square.Domain.TopicAggregate.ColumnEntity;
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

        public Task DeleteImagesAsync(
            IEnumerable<TopicImage> images,
            CancellationToken cancellationToken = default
        )
        {
            var keys = images.SelectMany(
                i =>
                    new[]
                    {
                        i.Url.AbsolutePath.TrimStart('/'),
                        i.ThumbnailUrl.AbsolutePath.TrimStart('/')
                    }
            );

            return _storage.DeleteImagesAsync(keys, cancellationToken);
        }

        public async Task<TopicImage> UploadImageAsync(
            IFormFile file,
            CancellationToken cancellationToken = default
        )
        {
            await using var image = file.OpenReadStream();

            var extension = await _processor.GetExtensionNameAsync(image, cancellationToken);

            var builder = builderPool.Get();

            builder
                .Append("images")
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

            return new(imageUrl, compressedImageUrl);
        }

        public async Task<IEnumerable<TopicImage>> UploadImagesAsync(
            IEnumerable<IFormFile> images,
            CancellationToken cancellationToken = default
        )
        {
            var topicImages = await Task.WhenAll(
                    images.Select(i => UploadImageAsync(i, cancellationToken))
                )
                .WaitAsync(cancellationToken);

            return topicImages.AsEnumerable();
        }
    }
}
