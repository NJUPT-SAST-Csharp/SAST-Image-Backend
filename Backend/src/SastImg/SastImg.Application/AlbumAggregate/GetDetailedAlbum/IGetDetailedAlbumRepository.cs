using Identity;
using SastImg.Domain.AlbumAggregate.AlbumEntity;

namespace SastImg.Application.AlbumAggregate.GetDetailedAlbum;

public interface IGetDetailedAlbumRepository
{
    Task<DetailedAlbumDto?> GetDetailedAlbumByUserAsync(
        AlbumId albumId,
        UserId requesterId,
        CancellationToken cancellationToken = default
    );

    Task<DetailedAlbumDto?> GetDetailedAlbumByAdminAsync(
        AlbumId albumId,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Provide album accessible to anonymous users.
    /// </summary>
    /// <remarks>Used only in cache.</remarks>
    Task<DetailedAlbumDto?> GetDetailedAlbumByAnonymousAsync(
        AlbumId albumId,
        CancellationToken cancellationToken = default
    );
}
