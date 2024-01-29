using SastImg.Domain;
using SastImg.Domain.AlbumAggregate.AlbumEntity;

namespace SastImg.Application.AlbumServices.GetAlbum
{
    public interface IGetAlbumRepository
    {
        Task<DetailedAlbumDto?> GetAlbumByUserAsync(
            AlbumId albumId,
            UserId requesterId,
            CancellationToken cancellationToken = default
        );

        Task<DetailedAlbumDto?> GetAlbumByAdminAsync(
            AlbumId albumId,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Provide album accessible to anonymous users.
        /// </summary>
        /// <remarks>Used only in cache.</remarks>
        Task<DetailedAlbumDto?> GetAlbumByAnonymousAsync(
            AlbumId albumId,
            CancellationToken cancellationToken = default
        );
    }
}
