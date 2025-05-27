using Identity;
using SastImg.Domain.CategoryEntity;

namespace SastImg.Application.AlbumServices.SearchAlbums;

public interface ISearchAlbumsRepository
{
    public Task<IEnumerable<SearchAlbumDto>> SearchAlbumsByAdminAsync(
        CategoryId categoryId,
        string title,
        int page,
        CancellationToken cancellationToken = default
    );
    public Task<IEnumerable<SearchAlbumDto>> SearchAlbumsByUserAsync(
        CategoryId categoryId,
        string title,
        int page,
        UserId requesterId,
        CancellationToken cancellationToken = default
    );
}
