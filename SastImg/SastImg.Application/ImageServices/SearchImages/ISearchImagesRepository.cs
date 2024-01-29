using SastImg.Domain;
using SastImg.Domain.CategoryEntity;
using SastImg.Domain.TagEntity;

namespace SastImg.Application.ImageServices.SearchImages
{
    public interface ISearchImagesRepository
    {
        public Task<IEnumerable<SearchedImageDto>> SearchImagesByAdminAsync(
            int page,
            CategoryId categoryId,
            TagId[] tags,
            CancellationToken cancellationToken = default
        );

        public Task<IEnumerable<SearchedImageDto>> SearchImagesByUserAsync(
            int page,
            CategoryId categoryId,
            TagId[] tags,
            UserId requesterId,
            CancellationToken cancellationToken = default
        );
    }
}
