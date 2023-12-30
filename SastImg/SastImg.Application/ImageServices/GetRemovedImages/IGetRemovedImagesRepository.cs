using SastImg.Application.ImageServices.GetImages;

namespace SastImg.Application.ImageServices.GetRemovedImages
{
    public interface IGetRemovedImagesRepository
    {
        public Task<IEnumerable<ImageDto>> GetImagesByUserAsync(
            long requesterId,
            CancellationToken cancellationToken = default
        );

        public Task<IEnumerable<ImageDto>> GetImagesByAdminAsync(
            long authorId,
            CancellationToken cancellationToken = default
        );
    }
}
