using Primitives.Entity;
using SNS.Domain.UserAggregate.UserEntity;

namespace SNS.Domain.AlbumEntity
{
    public sealed class Album : EntityBase<AlbumId>
    {
        private Album()
            : base(default) { }

        private readonly UserId _authorId;

        private bool _isAvailable;
    }
}
