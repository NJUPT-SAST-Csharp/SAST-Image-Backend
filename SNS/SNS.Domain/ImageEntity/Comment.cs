using SNS.Domain.UserEntity;

namespace SNS.Domain.ImageAggregate.ImageEntity
{
    public sealed record class Comment(ImageId ImageId, UserId CommenterId, string Content)
    {
        public DateTime CommentAt { get; } = DateTime.UtcNow;
    }
}
