using Identity;

namespace SastImg.Application.AlbumAggregate.GetRemovedAlbums;

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
