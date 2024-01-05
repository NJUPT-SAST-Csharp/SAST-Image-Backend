using Primitives.Entity;

namespace SNS.Domain.AlbumEntity
{
    public sealed class Album : EntityBase<long>
    {
        private Album()
            : base(default) { }

        private readonly long _authorId;

        private bool _isAvailable;
    }
}
