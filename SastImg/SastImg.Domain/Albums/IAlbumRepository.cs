namespace SastImg.Domain.Albums
{
    public interface IAlbumRepository
    {
        #region Album

        public Task RestoreRemovedAlbumAsync(
            long albumId,
            CancellationToken cancellationToken = default
        );

        public Task DeleteAllRemovedAlbumsAsync(CancellationToken cancellationToken = default);

        public Task<long> CreateAlbumAsync(
            long userId,
            string title,
            string description,
            Accessibility accessibility,
            CancellationToken cancellationToken = default
        );

        public Task UpdateAlbumAsync(
            long albumId,
            string title,
            string description,
            Accessibility accessibility,
            CancellationToken cancellationToken = default
        );

        public Task RemoveAlbumAsync(long albumId, CancellationToken cancellationToken = default);

        #endregion

        #region Image

        public Task DeleteAllRemovedImagesAsync(CancellationToken cancellationToken = default);

        public Task RestoreRemovedAlbumImageAsync(
            long albumId,
            long imageId,
            CancellationToken cancellationToken = default
        );

        public Task AddAlbumImageAsync(
            long albumId,
            string title,
            string description,
            Uri imageUri,
            IEnumerable<long> tags,
            CancellationToken cancellationToken = default
        );

        public Task RemoveImageFromAlbumAsync(
            long albumId,
            long imageId,
            CancellationToken cancellationToken = default
        );

        public Task UpdateAlbumImageAsync(
            long albumId,
            long imageId,
            string title,
            string description,
            IEnumerable<long> tags,
            CancellationToken cancellationToken = default
        );

        #endregion
    }
}
