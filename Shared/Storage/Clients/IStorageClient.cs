using Microsoft.AspNetCore.Http;

namespace Storage.Clients
{
    public interface IStorageClient
    {
        public Task<Uri> UploadImageAsync(
            IFormFile file,
            string key,
            CancellationToken cancellationToken = default
        );

        public Task DeleteImagesAsync(
            IEnumerable<Uri> paths,
            CancellationToken cancellationToken = default
        );

        public Task<Stream?> GetImageAsync(Uri url, CancellationToken cancellationToken = default);
    }
}
