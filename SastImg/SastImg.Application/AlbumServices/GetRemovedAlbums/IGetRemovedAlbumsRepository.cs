using Identity;

namespace SastImg.Application.AlbumServices.GetRemovedAlbums;

public interface IGetRemovedAlbumsRepository
{
    public Task<IEnumerable<RemovedAlbumDto>> GetRemovedAlbumsByAdminAsync(
        UserId authorId,
        CancellationToken cancellationToken = default
    );

    public Task<IEnumerable<RemovedAlbumDto>> GetRemovedAlbumsByUserAsync(
        UserId requesterId,
        CancellationToken cancellationToken = default
    );
}
