using SastImg.Application.ImageServices.GetImages;

namespace SastImg.Application.ImageServices.SearchImages
{
    public interface ISearchImagesRepository
    {
        public Task<IEnumerable<ImageDto>> SearchImagesByAdminAsync(
            int page,
            long categoryId,
            SearchOrder order,
            long[] tags,
            CancellationToken cancellationToken = default
        );

        public Task<IEnumerable<ImageDto>> SearchImagesByUserAsync(
            int page,
            long categoryId,
            SearchOrder order,
            long[] tags,
            long requesterId,
            CancellationToken cancellationToken = default
        );
    }
}
