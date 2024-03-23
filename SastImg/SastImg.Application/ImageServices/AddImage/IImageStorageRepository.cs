using Microsoft.AspNetCore.Http;

namespace SastImg.Application.ImageServices.AddImage
{
    public interface IImageStorageRepository
    {
        public Task<(Uri, Uri)> UploadImageAsync(
            IFormFile file,
            CancellationToken cancellationToken = default
        );
    }
}
