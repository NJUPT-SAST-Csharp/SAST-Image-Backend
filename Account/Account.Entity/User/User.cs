using Account.Entity.User.Options;
using Shared.Utilities;

namespace Account.Entity.User
{
    public sealed class User
    {
        private User() { }

        public required long Id { get; init; }
        public required string Username { get; init; } = string.Empty;
        public required byte[] PasswordHash { get; init; }
        public required string Email { get; init; } = string.Empty;

        public Profile Profile { get; set; } = new();

        public static User CreateUser(CreateUserOptions options)
        {
            return new User()
            {
                Id = SnowFlakeIdGenerator.NewId,
                Username = options.Username,
                PasswordHash = options.PasswordHash,
                Email = options.Email
            };
        }

        public void EditProfile(Profile profile)
        {
            Profile = profile;
        }
    }
}
