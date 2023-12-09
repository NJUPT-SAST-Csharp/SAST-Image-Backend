using Account.Entity.RoleEntity;
using Shared.Utilities;

namespace Account.Entity.UserEntity
{
    public sealed class User
    {
        private User() { }

        private readonly List<Role> roles = [];

        public long Id { get; private set; }
        public string Username { get; private set; }
        public string UsernameNormalized { get; private set; }
        public string Email { get; private set; }
        public byte[] PasswordHash { get; private set; }

        public IReadOnlyCollection<Role> Roles => roles;
        public Profile Profile { get; private set; }

        public User(string username, byte[] passwordHash, string email)
        {
            Id = SnowFlakeIdGenerator.NewId;
            Username = username;
            UsernameNormalized = username.ToUpperInvariant();
            Email = email.ToUpperInvariant();
            PasswordHash = passwordHash;
            Profile = new Profile(username, string.Empty);
        }

        public void EditProfile(string nickname, string bio, Uri? avatar, Uri? header) =>
            Profile = new(nickname, bio, avatar, header);

        public void ChangePassword(byte[] passwordHash) => PasswordHash = passwordHash;

        public void AddRole(Role role) => roles.Add(role);
    }
}
