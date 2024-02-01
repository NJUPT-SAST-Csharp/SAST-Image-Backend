using Primitives.Entity;
using SNS.Domain.UserEntity;

namespace SNS.Domain.AlbumEntity
{
    public sealed class Album : EntityBase<AlbumId>
    {
        private Album(AlbumId albumId)
            : base(albumId) { }

        private readonly ICollection<UserId> _subscribers;

        public static Album CreateNewAlbum(long albumId)
        {
            return new Album(new(albumId));
        }

        public void AddSubscriber(UserId subscriberId)
        {
            _subscribers.Add(subscriberId);
        }

        public void RemoveSubscriber(UserId subscriberId)
        {
            _subscribers.Remove(subscriberId);
        }
    }
}
