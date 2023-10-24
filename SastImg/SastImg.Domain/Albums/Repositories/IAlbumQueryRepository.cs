using SastImg.Domain.Albums.Images;

namespace SastImg.Domain.Albums.Repositories
{
    public interface IAlbumQueryRepository
    {
        #region Album
        public Task<Album?> GetAlbumAsync(
            long albumId,
            CancellationToken cancellationToken = default
        );

        public Task<Album[]> GetUserAlbumsAsync(
            long userId,
            CancellationToken cancellationToken = default
        );

        public Task<Album[]> GetRemovedAlbumsAsync(CancellationToken cancellationToken = default);

        public Task<Album[]> GetUserRemovedAlbumsAsync(
            long userId,
            CancellationToken cancellationToken = default
        );

        #endregion

        #region Image

        Task<Image[]> GetAlbumImagesAsync(
            long albumId,
            CancellationToken cancellationToken = default
        );

        Task<Image> GetImageAsync(long imageId, CancellationToken cancellationToken = default);

        public Task<Image[]> GetAlbumRemovedImagesAsync(
            long albumId,
            CancellationToken cancellationToken = default
        );

        #endregion
    }
}
