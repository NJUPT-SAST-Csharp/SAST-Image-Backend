using Primitives.Entity;
using Shared.Primitives;
using SNS.Domain.AlbumEntity;
using SNS.Domain.ImageAggregate.CommentEntity;
using SNS.Domain.UserEntity;

namespace SNS.Domain.ImageAggregate.ImageEntity
{
    public sealed class Image : EntityBase<ImageId>, IAggregateRoot<Image>
    {
        private Image(ImageId imageId, UserId authorId, AlbumId albumId)
            : base(imageId)
        {
            _authorId = authorId;
            _albumId = albumId;
        }

        private readonly List<UserId> _likedBy = [];
        private readonly List<UserId> _favouritedBy = [];
        private readonly List<Comment> _comments = [];
        private readonly AlbumId _albumId;
        private readonly UserId _authorId;

        public static Image CreateNewImage(long imageId, UserId authorId, AlbumId albumId)
        {
            return new Image(new(imageId), authorId, albumId);
        }

        public CommentId AddComment(UserId commenter, string content)
        {
            var comment = Comment.CreateNewComment(Id, commenter, content);
            _comments.Add(comment);
            return comment.Id;
        }

        public void DeleteComment(CommentId commentId)
        {
            var comment = _comments.First(comment => comment.Id == commentId);
            _comments.Remove(comment);
        }
    }
}
