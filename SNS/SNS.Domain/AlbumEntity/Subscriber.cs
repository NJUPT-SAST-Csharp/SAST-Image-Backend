using SNS.Domain.UserEntity;

namespace SNS.Domain.AlbumEntity
{
    public sealed class Subscriber
    {
        private Subscriber() { }

        public Subscriber(AlbumId albumId, UserId subscriberId)
        {
            AlbumId = albumId;
            SubscriberId = subscriberId;
        }

        public AlbumId AlbumId { get; init; }
        public UserId SubscriberId { get; init; }
    }
}
