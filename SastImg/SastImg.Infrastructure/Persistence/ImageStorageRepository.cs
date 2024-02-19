using Microsoft.AspNetCore.Http;
using SastImg.Application.ImageServices.AddImage;
using Storage.Clients;

namespace SastImg.Infrastructure.Persistence
{
    internal sealed class ImageStorageRepository(IStorageClientFactory factory)
        : IImageStorageRepository
    {
        private readonly ImageClient _client = factory.GetImageClient();

        public Task<Uri> UploadImageAsync(
            IFormFile file,
            CancellationToken cancellationToken = default
        )
        {
            return _client.UploadImageAsync(file, cancellationToken);
        }
    }
}
