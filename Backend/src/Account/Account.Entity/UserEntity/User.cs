using Account.Entity.RoleEntity;
using Utilities;

namespace Account.Entity.UserEntity
{
    public sealed class User
    {
        private User() { }

        private readonly List<Role> _roles = [];

        public long Id { get; private set; }
        public string Username { get; private set; }
        public string UsernameNormalized { get; private set; }
        public string Email { get; private set; }
        public byte[] PasswordHash { get; private set; }
        public byte[] PasswordSalt { get; private set; }

        public DateTime RegisteredAt { get; private init; }
        public DateTime LoginAt { get; private set; }

        public IReadOnlyCollection<Role> Roles => _roles;

        public User(string username, string password, string email)
        {
            Id = SnowFlakeIdGenerator.NewId;
            Username = username;
            UsernameNormalized = username.ToUpperInvariant();
            Email = email.ToUpperInvariant();
            PasswordSalt = Argon2Hasher.RandomSalt;
            PasswordHash = Argon2Hasher.Hash(password, PasswordSalt);
            RegisteredAt = DateTime.UtcNow;
        }

        public void ResetPassword(string password)
        {
            Argon2Hasher.RegenerateSalt(PasswordSalt);
            Argon2Hasher.Hash(password, PasswordSalt);
        }

        public void AddRole(Role role) => _roles.Add(role);

        public bool Login(string password)
        {
            bool isValid = Argon2Hasher.Validate(password, PasswordHash, PasswordSalt);
            if (isValid)
            {
                LoginAt = DateTime.UtcNow;
            }
            return isValid;
        }
    }
}
