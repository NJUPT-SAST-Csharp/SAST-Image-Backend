namespace Account.Entity.User
{
    public sealed class Profile
    {
        public string Nickname { get; set; } = string.Empty;
        public string Biography { get; set; } = string.Empty;
        public Uri? Avatar { get; set; } = null;
        public Uri? Header { get; set; } = null;
    }
}
