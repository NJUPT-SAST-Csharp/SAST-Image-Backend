using Shared.Primitives;
using Shared.Utilities;

namespace User.Domain
{
    public sealed class User(string linkId) : AggregateRoot<long>(SnowFlakeIdGenerator.NewId)
    {
        private readonly List<User> following =  [ ];
        private readonly List<User> followers =  [ ];

        #region Properties

        public string LinkId { get; private init; } = linkId;

        public string Email { get; private set; } = string.Empty;

        public string Nickname { get; private set; } = string.Empty;

        public string Biography { get; private set; } = string.Empty;

        public Uri? Avatar { get; set; } = null;

        public Uri? Header { get; set; } = null;

        #endregion

        #region Methods

        public void Follow(User user) => following.Add(user);

        public void Unfollow(User user) => following.Remove(user);

        public User? GetFollowerById(long id) => followers.FirstOrDefault(user => user.Id == id);

        public User? GetFollowingById(long id) => following.FirstOrDefault(user => user.Id == id);

        #endregion
    }
}
