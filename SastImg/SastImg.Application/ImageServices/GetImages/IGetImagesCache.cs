namespace SastImg.Application.ImageServices.GetImages
{
    public interface IGetImagesCache
    {
        public Task<IEnumerable<ImageDto>> GetImagesAsync(
            long albumId,
            CancellationToken cancellationToken = default
        );

        public Task<IEnumerable<ImageDto>> RemoveImagesAsync(
            long albumId,
            CancellationToken cancellationToken = default
        );
    }
}
