using SNS.Domain.ImageAggregate.ImageEntity;

namespace SNS.Domain.ImageAggregate
{
    public interface IImageDomainService
    {
        public Task<ImageId> CreateNewImageAsync(
            Image image,
            CancellationToken cancellationToken = default
        );

        public Task<Image> GetImageByIdAsync(
            ImageId id,
            CancellationToken cancellationToken = default
        );
    }
}
