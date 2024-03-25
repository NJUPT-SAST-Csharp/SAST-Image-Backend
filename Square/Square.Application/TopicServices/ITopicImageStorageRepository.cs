using Microsoft.AspNetCore.Http;
using Square.Domain.TopicAggregate.ColumnEntity;

namespace Square.Application.TopicServices
{
    public interface ITopicImageStorageRepository
    {
        public Task<TopicImage> UploadImageAsync(
            IFormFile file,
            CancellationToken cancellationToken = default
        );

        public Task DeleteImagesAsync(
            IEnumerable<TopicImage> images,
            CancellationToken cancellationToken = default
        );

        public Task<IEnumerable<TopicImage>> UploadImagesAsync(
            IEnumerable<IFormFile> images,
            CancellationToken cancellationToken = default
        );
    }
}
