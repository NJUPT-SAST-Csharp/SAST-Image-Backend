using Primitives.Entity;

namespace SNS.Domain.UserEntity
{
    public sealed class User : EntityBase<UserId>
    {
        private User()
            : base(default) { }

        private User(UserId userId)
            : base(userId) { }

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
    }
}
