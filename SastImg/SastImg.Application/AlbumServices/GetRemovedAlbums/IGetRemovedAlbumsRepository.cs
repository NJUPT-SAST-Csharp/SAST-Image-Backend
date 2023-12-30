using SastImg.Application.AlbumServices.GetAlbums;

namespace SastImg.Application.AlbumServices.GetRemovedAlbums
{
    public interface IGetRemovedAlbumsRepository
    {
        public Task<IEnumerable<AlbumDto>> GetAlbumsByAdminAsync(
            long authorId,
            CancellationToken cancellationToken = default
        );

        public Task<IEnumerable<AlbumDto>> GetAlbumsByUserAsync(
            long requesterId,
            CancellationToken cancellationToken = default
        );
    }
}
