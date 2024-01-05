using Primitives.Entity;

namespace SNS.Domain.CommentEntity
{
    public sealed class Comment : EntityBase<long>
    {
        private Comment()
            : base(default) { }
    }
}
