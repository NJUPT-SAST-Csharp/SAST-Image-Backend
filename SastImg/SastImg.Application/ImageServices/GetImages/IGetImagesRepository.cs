namespace SastImg.Application.ImageServices.GetImages
{
    public interface IGetImagesRepository
    {
        public Task<IEnumerable<AlbumImageDto>> GetImagesByUserAsync(
            long albumId,
            int page,
            CancellationToken cancellationToken = default
        );

        public Task<IEnumerable<AlbumImageDto>> GetImagesByAdminAsync(
            long albumId,
            int page,
            CancellationToken cancellationToken = default
        );

        public Task<IEnumerable<AlbumImageDto>> GetImagesByAnonymousAsync(
            long albumId,
            CancellationToken cancellationToken = default
        );
    }
}
