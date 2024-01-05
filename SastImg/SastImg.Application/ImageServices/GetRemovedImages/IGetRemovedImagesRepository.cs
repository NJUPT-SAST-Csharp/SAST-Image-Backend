using SastImg.Application.ImageServices.GetImages;

namespace SastImg.Application.ImageServices.GetRemovedImages
{
    public interface IGetRemovedImagesRepository
    {
        public Task<IEnumerable<AlbumImageDto>> GetImagesByUserAsync(
            long requesterId,
            CancellationToken cancellationToken = default
        );

        public Task<IEnumerable<AlbumImageDto>> GetImagesByAdminAsync(
            long authorId,
            CancellationToken cancellationToken = default
        );
    }
}
