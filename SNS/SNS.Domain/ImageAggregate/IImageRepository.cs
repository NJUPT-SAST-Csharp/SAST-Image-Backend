using SNS.Domain.ImageAggregate.ImageEntity;

namespace SNS.Domain.ImageAggregate
{
    public interface IImageRepository
    {
        public Task<ImageId> AddNewImageAsync(
            Image image,
            CancellationToken cancellationToken = default
        );
    }
}
