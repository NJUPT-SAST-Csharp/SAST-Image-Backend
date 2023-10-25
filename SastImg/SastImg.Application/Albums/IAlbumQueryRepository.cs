using SastImg.Application.Albums.Dtos;
using SastImg.Application.Albums.Images.Dtos;

namespace SastImg.Application.Albums
{
    // TODO: Move the IQuery part to the application.
    public interface IAlbumQueryRepository
    {
        #region Album
        public Task<AlbumDetailsDto?> GetAlbumDetailsAsync(
            long albumId,
            CancellationToken cancellationToken = default
        );

        public Task<IEnumerable<AlbumDto>> GetUserAlbumsAsync(
            long userId,
            CancellationToken cancellationToken = default
        );

        public Task<IEnumerable<AlbumDto>> GetRemovedAlbumsAsync(
            CancellationToken cancellationToken = default
        );

        public Task<IEnumerable<AlbumDto>> GetUserRemovedAlbumsAsync(
            long userId,
            CancellationToken cancellationToken = default
        );

        #endregion

        #region Image

        public Task<IEnumerable<ImageDto>> GetAlbumImagesAsync(
            long albumId,
            CancellationToken cancellationToken = default
        );

        public Task<ImageDetailsDto> GetImageDetailsAsync(
            long imageId,
            CancellationToken cancellationToken = default
        );

        public Task<IEnumerable<ImageDto>> GetAlbumRemovedImagesAsync(
            long albumId,
            CancellationToken cancellationToken = default
        );

        #endregion
    }
}
