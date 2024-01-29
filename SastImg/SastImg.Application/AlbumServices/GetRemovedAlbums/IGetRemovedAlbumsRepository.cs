using SastImg.Application.AlbumServices.GetAlbums;
using SastImg.Domain;

namespace SastImg.Application.AlbumServices.GetRemovedAlbums
{
    public interface IGetRemovedAlbumsRepository
    {
        public Task<IEnumerable<AlbumDto>> GetAlbumsByAdminAsync(
            UserId authorId,
            CancellationToken cancellationToken = default
        );

        public Task<IEnumerable<AlbumDto>> GetAlbumsByUserAsync(
            UserId requesterId,
            CancellationToken cancellationToken = default
        );
    }
}
