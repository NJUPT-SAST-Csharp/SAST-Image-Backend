using Primitives.Entity;

namespace SNS.Domain.UserEntity
{
    public sealed class User : EntityBase<UserId>
    {
        private User()
            : base(default) { }

        private User(UserId userId)
            : base(userId) { }

        public static User CreateNewUser(UserId userId)
        {
            var user = new User(userId);
            return user;
        }

        private readonly List<User> _following = [];

        public void Follow(UserId followingId)
        {
            if (_following.Any(f => f.Id == followingId))
                return;

            _following.Add(new(followingId));
        }

        public void Unfollow(UserId followingId)
        {
            var following = _following.FirstOrDefault(f => f.Id == followingId);
            if (following is null)
                return;
            _following.Remove(following);
        }
    }
}
