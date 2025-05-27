using Identity;
using SastImg.Domain.AlbumTagEntity;
using SastImg.Domain.CategoryEntity;

namespace SastImg.Application.ImageServices.SearchImages;

public interface ISearchImagesRepository
{
    public Task<IEnumerable<SearchedImageDto>> SearchImagesByAdminAsync(
        int page,
        CategoryId categoryId,
        ImageTagId[] tags,
        CancellationToken cancellationToken = default
    );

    public Task<IEnumerable<SearchedImageDto>> SearchImagesByUserAsync(
        int page,
        CategoryId categoryId,
        ImageTagId[] tags,
        UserId requesterId,
        CancellationToken cancellationToken = default
    );
}
