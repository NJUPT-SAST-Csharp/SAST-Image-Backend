using Primitives.Entity;
using Shared.Utilities;
using SNS.Domain.ImageAggregate.ImageEntity;
using SNS.Domain.UserAggregate.UserEntity;

namespace SNS.Domain.ImageAggregate.CommentEntity
{
    public sealed class Comment : EntityBase<CommentId>
    {
        private Comment(ImageId imageId, UserId authorId, string content)
            : base(new(SnowFlakeIdGenerator.NewId))
        {
            _imageId = imageId;
            _authorId = authorId;
            _content = content;
            _commentAt = DateTime.UtcNow;
        }

        private readonly UserId _authorId;
        private readonly ImageId _imageId;
        private readonly string _content;
        private readonly DateTime _commentAt;

        public static Comment CreateNewComment(ImageId imageId, UserId authorId, string content)
        {
            return new Comment(imageId, authorId, content);
        }
    }
}
