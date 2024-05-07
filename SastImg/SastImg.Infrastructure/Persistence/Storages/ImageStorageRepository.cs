using System.Text;
using Microsoft.AspNetCore.Http;
using SastImg.Application.ImageServices.AddImage;
using Storage.Clients;

namespace SastImg.Infrastructure.Persistence.Storages
{
    internal sealed class ImageStorageRepository(IStorageClient client, IProcessClient processor)
        : IImageStorageRepository
    {
        private readonly IStorageClient _client = client;
        private readonly IProcessClient _processor = processor;

        public async Task<(Uri, Uri)> UploadImageAsync(
            IFormFile file,
            CancellationToken cancellationToken = default
        )
        {
            await using var image = file.OpenReadStream();

            var extension = await _processor.GetExtensionNameAsync(image, cancellationToken);

            var key = new StringBuilder(64)
                .Append("images")
                .Append('/')
                .Append(DateTime.UtcNow.ToString("yyyy/MM/dd"))
                .Append('/')
                .Append(Guid.NewGuid().ToString("N"))
                .Append('.')
                .Append(extension)
                .ToString();

            var url = await _client.UploadImageAsync(image, key, cancellationToken);

            var compressedUrl = await _processor.CompressImageAsync(key, false, cancellationToken);

            return (url, compressedUrl);
        }
    }
}
