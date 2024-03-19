namespace SNS.Domain.ImageAggregate.ImageEntity
{
    public interface IImageRepository
    {
        public Task<ImageId> AddNewImageAsync(
            Image image,
            CancellationToken cancellationToken = default
        );
    }
}
