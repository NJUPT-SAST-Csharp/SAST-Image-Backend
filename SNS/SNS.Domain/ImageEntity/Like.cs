using SNS.Domain.UserEntity;

namespace SNS.Domain.ImageAggregate.ImageEntity
{
    public sealed record class Like(ImageId ImageId, UserId UserId)
    {
        public DateTime LikeAt { get; } = DateTime.UtcNow;
    }
}
