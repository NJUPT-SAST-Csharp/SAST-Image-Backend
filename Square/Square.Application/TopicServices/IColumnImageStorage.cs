using Microsoft.AspNetCore.Http;
using Square.Application.ColumnServices.Models;

namespace Square.Application.TopicServices
{
    public interface IColumnImageStorage
    {
        public Task<ColumnImage> UploadImageAsync(
            IFormFile file,
            CancellationToken cancellationToken = default
        );

        public Task DeleteImagesAsync(
            IEnumerable<ColumnImage> images,
            CancellationToken cancellationToken = default
        );

        public Task<IEnumerable<ColumnImage>> UploadImagesAsync(
            IEnumerable<IFormFile> images,
            CancellationToken cancellationToken = default
        );
    }
}
