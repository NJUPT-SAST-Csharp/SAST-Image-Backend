using Primitives.Entity;
using Shared.Primitives;
using SNS.Domain.AlbumEntity;
using SNS.Domain.CommentEntity;
using SNS.Domain.ImageEntity;

namespace SNS.Domain.UserEntity
{
    public sealed class User() : EntityBase<long>(default), IAggregateRoot<User>
    {
        private string _username;
        private string _nickname;
        private string _biography;

        private readonly List<User> _following;
        private readonly List<User> _followers;
        private readonly List<Album> _subscribing;
        private readonly List<Image> _favorites;
        private readonly List<Comment> _comments;

        public void AddFollowing(User user)
        {
            _following.Add(user);
        }

        public void AddFollower(User user)
        {
            _followers.Add(user);
        }
    }
}
