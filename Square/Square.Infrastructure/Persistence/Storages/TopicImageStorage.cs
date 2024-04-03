using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.ObjectPool;
using Square.Application.ColumnServices.Models;
using Square.Application.TopicServices;
using Storage.Clients;

namespace Square.Infrastructure.Persistence.Storages
{
    internal class TopicImageStorage(
        IStorageClient storage,
        IProcessClient processor,
        ObjectPool<StringBuilder> builderPool
    ) : IColumnImageStorage
    {
        private readonly IStorageClient _storage = storage;
        private readonly IProcessClient _processor = processor;

        public Task DeleteImagesAsync(
            IEnumerable<ColumnImage> images,
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

        public async Task<ColumnImage> UploadImageAsync(
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

        public async Task<IEnumerable<ColumnImage>> UploadImagesAsync(
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
