using SastImg.Domain;

namespace SastImg.Application.AlbumServices.GetAlbums
{
    public interface IGetUserAlbumsRepository
    {
        public Task<IEnumerable<UserAlbumDto>> GetUserAlbumsByAdminAsync(
            UserId authorId,
            CancellationToken cancellationToken = default
        );

        public Task<IEnumerable<UserAlbumDto>> GetUserAlbumsByUserAsync(
            UserId authorId,
            UserId requesterId,
            CancellationToken cancellationToken = default
        );
    }
}
