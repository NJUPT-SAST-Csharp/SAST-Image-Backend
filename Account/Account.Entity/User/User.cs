using Shared.Utilities;

namespace Account.Entity.User
{
    public sealed class User
    {
        private User() { }

        public long Id { get; init; }
        public string Username { get; init; } = string.Empty;
        public byte[] PasswordHash { get; init; } = [];
        public string Email { get; init; } = string.Empty;
        public Profile Profile { get; set; } = new();

        public static long CreateUser(string username, byte[] passwordHash, string email)
        {
            var user = new User()
            {
                Id = SnowFlakeIdGenerator.NewId,
                Username = username,
                PasswordHash = passwordHash,
                Email = email
            };
            return user.Id;
        }
    }
}
