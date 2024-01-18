using Primitives.Entity;

namespace SNS.Domain.ImageAggregate.CommentEntity
{
    public sealed class Comment : EntityBase<long>
    {
        private Comment()
            : base(default) { }

        private readonly long _authorId;
        private readonly long _imageId;
        private readonly string _content;
    }
}
