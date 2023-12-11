using Account.Entity.RoleEntity;
using Shared.Utilities;

namespace Account.Entity.UserEntity
{
    public sealed class User
    {
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        private User() { }
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

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

        public bool ChangePassword(byte[] formerPasswordHash, byte[] newPasswordHash)
        {
            if (formerPasswordHash.SequenceEqual(PasswordHash))
            {
                PasswordHash = newPasswordHash;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ResetPassword(byte[] passwordHash) => PasswordHash = passwordHash;

        public void AddRole(Role role) => roles.Add(role);
    }
}
