using SNS.Domain.UserEntity;

namespace SNS.Domain.AlbumEntity
{
    public sealed record class Subscriber(AlbumId AlbumId, UserId SubscriberId)
    {
        public DateTime SubscribeAt { get; } = DateTime.UtcNow;
    }
}
