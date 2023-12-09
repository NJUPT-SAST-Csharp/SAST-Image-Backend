namespace Account.Entity.UserEntity
{
    public sealed class Profile
    {
        internal Profile(string nickname, string biography, Uri? avatar = null, Uri? header = null)
        {
            Nickname = nickname;
            Biography = biography;
            Avatar = avatar;
            Header = header;
        }

        public string Nickname { get; private set; }
        public string Biography { get; private set; }
        public Uri? Avatar { get; private set; }
        public Uri? Header { get; private set; }
    }
}
