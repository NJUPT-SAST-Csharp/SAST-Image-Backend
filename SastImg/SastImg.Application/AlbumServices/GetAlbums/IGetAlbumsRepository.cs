using SastImg.Domain;
using SastImg.Domain.CategoryEntity;

namespace SastImg.Application.AlbumServices.GetAlbums
{
    public interface IGetAlbumsRepository
    {
        public Task<IEnumerable<AlbumDto>> GetAlbumsByAdminAsync(
            CategoryId categoryId,
            int page,
            CancellationToken cancellationToken = default
        );

        public Task<IEnumerable<AlbumDto>> GetAlbumsByUserAsync(
            CategoryId categoryId,
            int page,
            UserId requester,
            CancellationToken cancellationToken = default
        );

        public Task<IEnumerable<AlbumDto>> GetAlbumsAnonymousAsync(
            CategoryId categoryId,
            CancellationToken cancellationToken = default
        );
    }
}
