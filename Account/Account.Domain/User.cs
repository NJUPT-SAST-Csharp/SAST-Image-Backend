using Shared.Primitives;
using Shared.Utilities;

namespace User.Domain
{
    public class User : AggregateRoot<long>
    {
        private readonly ICollection<User> following = new List<User>();
        private readonly ICollection<User> followers = new List<User>();

        public User(string linkId)
            : base(SnowFlakeIdGenerator.NewId)
        {
            LinkId = linkId;
        }

        #region Properties

        public string LinkId { get; private init; }

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
