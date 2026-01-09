using Identity;
using SastImg.Domain.AlbumAggregate.AlbumEntity;

namespace SastImg.Application.ImageServices.GetAlbumImages;

public interface IGetAlbumImagesRepository
{
    public Task<IEnumerable<AlbumImageDto>> GetImagesByUserAsync(
        AlbumId albumId,
        UserId requesterId,
        int page,
        CancellationToken cancellationToken = default
    );

    public Task<IEnumerable<AlbumImageDto>> GetImagesByAdminAsync(
        AlbumId albumId,
        int page,
        CancellationToken cancellationToken = default
    );

    public Task<IEnumerable<AlbumImageDto>> GetImagesByAnonymousAsync(
        AlbumId albumId,
        CancellationToken cancellationToken = default
    );
}
