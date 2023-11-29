namespace Account.Entity.User.Models
{
    public sealed class UserIdentity
    {
        public required long Id { get; init; }
        public required string Username { get; init; }
        public required byte[] PasswordHash { get; init; }
    }
}
