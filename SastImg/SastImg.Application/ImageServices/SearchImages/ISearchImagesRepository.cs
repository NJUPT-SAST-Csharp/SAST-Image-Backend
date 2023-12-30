using SastImg.Application.ImageServices.GetImages;
using SastImg.Infrastructure.QueryRepositories;

namespace SastImg.Application.ImageServices.SearchImages
{
    public interface ISearchImagesRepository
    {
        public Task<IEnumerable<ImageDto>> SearchImagesByAdminAsync(
            int page,
            long categoryId,
            OrderOptions order,
            long[] tags,
            CancellationToken cancellationToken = default
        );

        public Task<IEnumerable<ImageDto>> SearchImagesByUserAsync(
            int page,
            long categoryId,
            OrderOptions order,
            long[] tags,
            long requesterId,
            CancellationToken cancellationToken = default
        );
    }
}
