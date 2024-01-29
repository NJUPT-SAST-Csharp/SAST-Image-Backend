using SastImg.Application.AlbumServices.GetAlbums;
using SastImg.Domain;
using SastImg.Domain.CategoryEntity;

namespace SastImg.Application.AlbumServices.SearchAlbums
{
    public interface ISearchAlbumsRepository
    {
        public Task<IEnumerable<AlbumDto>> SearchAlbumsByAdminAsync(
            CategoryId categoryId,
            string title,
            int page,
            CancellationToken cancellationToken = default
        );
        public Task<IEnumerable<AlbumDto>> SearchAlbumsByUserAsync(
            CategoryId categoryId,
            string title,
            int page,
            UserId requesterId,
            CancellationToken cancellationToken = default
        );
    }
}
