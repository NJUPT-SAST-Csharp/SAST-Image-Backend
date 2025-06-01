using Identity;
using SastImg.Domain.AlbumAggregate.AlbumEntity;
using SastImg.Domain.AlbumAggregate.ImageEntity;

namespace SastImg.Application.ImageServices.GetImage;

public interface IGetImageRepository
{
    public Task<DetailedImageDto?> GetImageByUserAsync(
        AlbumId albumId,
        ImageId imageId,
        UserId requesterId,
        CancellationToken cancellationToken = default
    );

    public Task<DetailedImageDto?> GetImageByAdminAsync(
        AlbumId albumId,
        ImageId imageId,
        CancellationToken cancellationToken = default
    );

    public Task<DetailedImageDto?> GetImageByAnonymousAsync(
        AlbumId albumId,
        ImageId imageId,
        CancellationToken cancellationToken = default
    );
}
