using SastImg.Domain.Entities;
using SastImg.Domain.Enums;

namespace SastImg.Domain.Repositories
{
    public interface IAlbumRepository
    {
        #region Album
        public Task<Album?> GetAlbumByIdAsync(
            long albumId,
            CancellationToken cancellationToken = default
        );

        public Task<Album[]> GetAlbumsByAuthorIdAsync(
            long albumId,
            CancellationToken cancellationToken = default
        );

        public Task<Album[]> GetRemovedAlbumsAsync(CancellationToken cancellationToken = default);

        public Task RemoveAlbumByIdAsync(
            long albumId,
            CancellationToken cancellationToken = default
        );

        public Task<int> DeleteAllRemovedAlbumsAsync(CancellationToken cancellationToken = default);

        public Task<int> DeleteAllRemovedImagesAsync(CancellationToken cancellationToken = default);

        public Task<long> CreateAlbumAsync(
            Album album,
            CancellationToken cancellationToken = default
        );

        public Task UpdateAlbumInfoAsync(
            long albumId,
            string title,
            string description,
            Accessibility accessibility,
            CancellationToken cancellationToken = default
        );

        #endregion

        #region Image

        public Task AddImageToAlbumAsync(
            long albumId,
            Image image,
            CancellationToken cancellationToken = default
        );

        public Task RemoveImageFromAlbumAsync(
            long albumId,
            long imageId,
            CancellationToken cancellationToken = default
        );

        public Task UpdateImageAsync(
            long albumId,
            long imageId,
            CancellationToken cancellationToken = default
        );

        #endregion
    }
}
