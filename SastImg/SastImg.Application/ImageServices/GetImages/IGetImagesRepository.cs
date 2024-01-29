using SastImg.Domain.AlbumAggregate.AlbumEntity;

namespace SastImg.Application.ImageServices.GetImages
{
    public interface IGetImagesRepository
    {
        public Task<IEnumerable<AlbumImageDto>> GetImagesByUserAsync(
            AlbumId albumId,
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
}
