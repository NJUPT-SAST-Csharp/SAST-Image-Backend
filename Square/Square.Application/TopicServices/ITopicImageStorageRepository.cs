using Microsoft.AspNetCore.Http;

namespace Square.Application.TopicServices
{
    public interface ITopicImageStorageRepository
    {
        public Task<(Uri, Uri)> UploadImageAsync(
            IFormFile file,
            CancellationToken cancellationToken = default
        );
    }
}
