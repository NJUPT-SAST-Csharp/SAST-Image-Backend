using Primitives.Entity;
using SNS.Domain.UserEntity;
using Utilities;

namespace SNS.Domain.ImageAggregate.CommentEntity
{
    public sealed class Comment : EntityBase<CommentId>
    {
        private Comment()
            : base(default) { }

        private Comment(UserId commenter, string content)
            : base(new(SnowFlakeIdGenerator.NewId))
        {
            _commenter = commenter;
            _content = content;
            _commentAt = DateTime.UtcNow;
        }

        private readonly UserId _commenter;
        private readonly string _content;
        private readonly DateTime _commentAt;

        internal static Comment CreateNewComment(UserId commenter, string content)
        {
            return new Comment(commenter, content);
        }
    }
}
