using Primitives.Entity;

namespace SNS.Domain.UserEntity
{
    public sealed class User : EntityBase<UserId>
    {
        private User(UserId userId)
            : base(userId) { }

        private string _nickname = "SASTer";
        private string _biography = string.Empty;

        private readonly List<User> _following;

        public static User CreateNewUser(long userId)
        {
            var user = new User(new(userId));
            return user;
        }

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
