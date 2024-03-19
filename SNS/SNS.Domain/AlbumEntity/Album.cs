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
        private readonly List<Subscriber> _subscribers = [];

        public static Album CreateNewAlbum(AlbumId albumId, UserId authorId)
        {
            return new Album(albumId, authorId);
        }

        public void AddSubscriber(UserId subscriberId)
        {
            _subscribers.Add(new(Id, subscriberId));
        }

        public void RemoveSubscriber(UserId subscriberId)
        {
            _subscribers.Remove(_subscribers.SingleOrDefault(s => s.SubscriberId == subscriberId)!);
        }
    }
}
