namespace SastImg.Application.ImageServices.GetImage
{
    public interface IGetImageRepository
    {
        public Task<DetailedImageDto?> GetImageByUserAsync(
            long imageId,
            long requesterId,
            CancellationToken cancellationToken = default
        );

        public Task<DetailedImageDto?> GetImageByAdminAsync(
            long imageId,
            CancellationToken cancellationToken = default
        );

        public Task<DetailedImageDto?> GetImageByAnonymousAsync(
            string imageId,
            CancellationToken cancellationToken = default
        );
    }
}
