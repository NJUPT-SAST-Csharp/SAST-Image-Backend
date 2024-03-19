using SNS.Domain.UserEntity;

namespace SNS.Domain.ImageAggregate.ImageEntity
{
    public sealed record class Favourite(ImageId ImageId, UserId UserId)
    {
        public DateTime FavouriteAt { get; } = DateTime.UtcNow;
    }
}
