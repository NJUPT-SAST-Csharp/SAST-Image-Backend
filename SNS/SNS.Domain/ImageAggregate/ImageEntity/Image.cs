using Primitives.Entity;

namespace SNS.Domain.ImageAggregate.ImageEntity
{
    public sealed class Image : EntityBase<ImageId>
    {
        private Image(ImageId imageId, long authorId, long albumId)
            : base(imageId)
        {
            _authorId = authorId;
            _albumId = albumId;
        }

        private long _albumId;
        private long _authorId;
        private int _likes;

        public static Image CreateNewImage(long imageId, long authorId, long albumId)
        {
            return new Image(new(imageId), authorId, albumId);
        }
    }
}
