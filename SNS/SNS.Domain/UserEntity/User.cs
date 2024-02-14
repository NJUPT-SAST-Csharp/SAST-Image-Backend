using Primitives.Entity;

namespace SNS.Domain.UserEntity
{
    public sealed class User : EntityBase<UserId>
    {
        private User()
            : base(default) { }

        private User(UserId userId)
            : base(userId) { }

        private string _nickname = "SASTer";
        private string _biography = string.Empty;
        private Uri _header;
        private Uri _avatar;

        private readonly IList<User> _following = [];

        public static User CreateNewUser(UserId userId)
        {
            var user = new User(userId);
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

        public void UpdateAvatar(Uri avatar)
        {
            _avatar = avatar;
        }

        public void UpdateHeader(Uri header)
        {
            _header = header;
        }
    }
}
