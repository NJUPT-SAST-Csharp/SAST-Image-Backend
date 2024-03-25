using Microsoft.AspNetCore.Http;
using Square.Domain.TopicAggregate.ColumnEntity;

namespace Square.Application.TopicServices
{
    public interface ITopicImageStorageRepository
    {
        public Task<(Uri, Uri)> UploadImageAsync(
            IFormFile file,
            CancellationToken cancellationToken = default
        );

        public Task DeleteImagesAsync(
            IEnumerable<TopicImage> images,
            CancellationToken cancellationToken = default
        );
    }
}
