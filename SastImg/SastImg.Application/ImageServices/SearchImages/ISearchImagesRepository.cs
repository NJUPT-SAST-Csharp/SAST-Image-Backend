using SastImg.Application.ImageServices.GetImages;

namespace SastImg.Application.ImageServices.SearchImages
{
    public interface ISearchImagesRepository
    {
        public Task<IEnumerable<ImageDto>> SearchImagesByAdminAsync(
            int page,
            SearchOrder order,
            long categoryId,
            long[] tags,
            CancellationToken cancellationToken = default
        );

        public Task<IEnumerable<ImageDto>> SearchImagesByUserAsync(
            int page,
            SearchOrder order,
            long categoryId,
            long[] tags,
            long requesterId,
            CancellationToken cancellationToken = default
        );
    }
}
