using Primitives.Entity;

namespace SNS.Domain.UserEntity
{
    public sealed class User : EntityBase<UserId>
    {
        private User()
            : base(default) { }

        private string _nickname;
        private string _biography;

        private readonly List<User> _following;

        public void Follow(User user)
        {
            _following.Add(user);
        }

        public void Unfollow(User user)
        {
            _following.Remove(user);
        }

        public void UpdateProfile(string nickname, string biography)
        {
            _nickname = nickname;
            _biography = biography;
        }
    }
}
