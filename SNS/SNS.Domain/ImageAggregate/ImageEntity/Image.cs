using Primitives.Entity;
using SNS.Domain.AlbumEntity;
using SNS.Domain.UserAggregate.UserEntity;

namespace SNS.Domain.ImageAggregate.ImageEntity
{
    public sealed class Image : EntityBase<ImageId>
    {
        private Image(ImageId imageId, UserId authorId, AlbumId albumId)
            : base(imageId)
        {
            _authorId = authorId;
            _albumId = albumId;
        }

        private readonly AlbumId _albumId;
        private readonly UserId _authorId;
        private int _likes;

        public static Image CreateNewImage(long imageId, long authorId, long albumId)
        {
            return new Image(new(imageId), new(authorId), new(albumId));
        }
    }
}
