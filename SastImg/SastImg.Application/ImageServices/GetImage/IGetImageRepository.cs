using SastImg.Domain;
using SastImg.Domain.AlbumAggregate.ImageEntity;

namespace SastImg.Application.ImageServices.GetImage
{
    public interface IGetImageRepository
    {
        public Task<DetailedImageDto?> GetImageByUserAsync(
            ImageId imageId,
            UserId requesterId,
            CancellationToken cancellationToken = default
        );

        public Task<DetailedImageDto?> GetImageByAdminAsync(
            ImageId imageId,
            CancellationToken cancellationToken = default
        );

        public Task<DetailedImageDto?> GetImageByAnonymousAsync(
            ImageId imageId,
            CancellationToken cancellationToken = default
        );
    }
}
