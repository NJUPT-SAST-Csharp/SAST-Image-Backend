namespace Account.Entity.User
{
    public sealed class Profile
    {
        public string Nickname { get; init; } = string.Empty;
        public string Biography { get; init; } = string.Empty;
        public Uri? Avatar { get; init; } = null;
        public Uri? Header { get; init; } = null;
    }
}
