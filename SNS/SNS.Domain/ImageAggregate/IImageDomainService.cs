using SNS.Domain.ImageAggregate.ImageEntity;

namespace SNS.Domain.ImageAggregate
{
    public interface IImageDomainService
    {
        public Task<ImageId> CreateNewImageAsync(
            long imageId,
            long authorId,
            long albumId,
            CancellationToken cancellationToken = default
        );
    }
}
