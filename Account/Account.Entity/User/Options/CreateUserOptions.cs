namespace Account.Entity.User.Options
{
    public readonly ref struct CreateUserOptions(string username, byte[] password, string email)
    {
        public readonly string Username { get; } = username;
        public readonly byte[] PasswordHash { get; } = password;
        public readonly string Email { get; } = email;
    }
}
