using SNS.Domain.UserEntity;

namespace SNS.Domain.ImageAggregate.ImageEntity
{
    public sealed class Like
    {
        private Like() { }

        public Like(ImageId imageId, UserId userId)
        {
            ImageId = imageId;
            UserId = userId;
        }

        public ImageId ImageId { get; init; }
        public UserId UserId { get; init; }
    }
}
