using Primitives.Entity;
using SNS.Domain.AlbumEntity;
using SNS.Domain.UserEntity;

namespace SNS.Domain.ImageAggregate.ImageEntity
{
    public sealed class Image : EntityBase<ImageId>
    {
        private Image()
            : base(default) { }

        private Image(ImageId imageId, UserId authorId, AlbumId albumId)
            : base(imageId)
        {
            _authorId = authorId;
            _albumId = albumId;
        }

        public static Image CreateNewImage(ImageId imageId, UserId authorId, AlbumId albumId)
        {
            return new Image(imageId, authorId, albumId);
            //TODO: Add domain event
        }

        #region Fields

        private readonly List<Like> _likes = [];
        private readonly List<Favourite> _favourites = [];
        private readonly List<Comment> _comments = [];

        private readonly AlbumId _albumId;
        private readonly UserId _authorId;

        #endregion

        #region Methods

        public void Comment(UserId commenter, string content)
        {
            var comment = new Comment(Id, commenter, content);
            _comments.Add(comment);
        }

        public void DeleteComment(UserId commenterId)
        {
            var comment = _comments.First(comment => comment.CommenterId == commenterId);
            _comments.Remove(comment);
        }

        #endregion
    }
}
