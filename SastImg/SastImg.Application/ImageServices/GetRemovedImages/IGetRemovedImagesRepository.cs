using SastImg.Application.ImageServices.GetImages;
using SastImg.Domain;

namespace SastImg.Application.ImageServices.GetRemovedImages
{
    public interface IGetRemovedImagesRepository
    {
        public Task<IEnumerable<AlbumImageDto>> GetImagesByUserAsync(
            UserId requesterId,
            CancellationToken cancellationToken = default
        );

        public Task<IEnumerable<AlbumImageDto>> GetImagesByAdminAsync(
            UserId authorId,
            CancellationToken cancellationToken = default
        );
    }
}
