using Identity;

namespace SastImg.Application.AlbumAggregate.GetUserAlbums;

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
