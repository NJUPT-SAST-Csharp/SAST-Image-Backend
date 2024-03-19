using Primitives.Entity;
using SNS.Domain.UserEntity;

namespace SNS.Domain.AlbumEntity
{
    public sealed class Album : EntityBase<AlbumId>
    {
        private Album()
            : base(default) { }

        private Album(AlbumId albumId, UserId authorId)
            : base(albumId)
        {
            _authorId = authorId;
        }

        private readonly UserId _authorId;
        private readonly List<Subscribe> _subscribers = [];

        public static Album CreateNewAlbum(AlbumId albumId, UserId authorId)
        {
            return new Album(albumId, authorId);
        }

        public void Subscribe(UserId subscriberId)
        {
            if (_subscribers.Any(s => s.SubscriberId == subscriberId))
                return;

            _subscribers.Add(new(Id, subscriberId));
        }

        public void Unsubscribe(UserId subscriberId)
        {
            var subscriber = _subscribers.FirstOrDefault(s => s.SubscriberId == subscriberId);

            if (subscriber is null)
                return;

            _subscribers.Remove(subscriber);
        }
    }
}
