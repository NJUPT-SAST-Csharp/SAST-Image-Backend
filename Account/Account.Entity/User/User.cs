using Shared.Utilities;

namespace Account.Entity.User
{
    public sealed class User
    {
        private User() { }

        public long Id { get; private set; }
        public string Username { get; private set; } = string.Empty;
        public byte[] PasswordHash { get; private set; }
        public string Email { get; private set; } = string.Empty;

        public Profile Profile { get; set; } = new();

        public User(string username, byte[] passwordHash, string email)
        {
            Id = SnowFlakeIdGenerator.NewId;
            Username = username;
            PasswordHash = passwordHash;
            Email = email;
        }

        public void EditProfile(Profile profile)
        {
            Profile = profile;
        }
    }
}
