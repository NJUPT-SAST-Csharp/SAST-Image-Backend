namespace SastImg.Application.ImageServices.GetImages
{
    public interface IGetImagesRepository
    {
        public Task<IEnumerable<ImageDto>> GetImagesByUserAsync(
            long albumId,
            int page,
            CancellationToken cancellationToken = default
        );

        public Task<IEnumerable<ImageDto>> GetImagesByAdminAsync(
            long albumId,
            int page,
            CancellationToken cancellationToken = default
        );
    }
}
