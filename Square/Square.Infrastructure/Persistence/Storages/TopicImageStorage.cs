using System.Text;
using Microsoft.AspNetCore.Http;
using Square.Application.ColumnServices.Models;
using Square.Application.TopicServices;
using Storage.Clients;

namespace Square.Infrastructure.Persistence.Storages
{
    internal class TopicImageStorage(IStorageClient storage, IProcessClient processor)
        : IColumnImageStorage
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

            string mainKey = new StringBuilder(64)
                .Append("images")
                .Append('/')
                .Append(DateTime.UtcNow.ToString("yyyy/MM/dd"))
                .Append('/')
                .Append(Guid.NewGuid().ToString("N"))
                .Append('.')
                .Append(extension)
                .ToString();

            var imageUrl = await _storage.UploadImageAsync(image, mainKey, cancellationToken);

            var compressedImageUrl = await _processor.CompressImageAsync(
                mainKey,
                false,
                cancellationToken
            );

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
