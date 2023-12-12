namespace Account.Entity.UserEntity
{
    public sealed class Profile
    {
        internal Profile(
            long id,
            string nickname,
            string biography,
            Uri? avatar = null,
            Uri? header = null
        )
        {
            Id = id;
            Nickname = nickname;
            Biography = biography;
            Avatar = avatar;
            Header = header;
        }

        public long Id { get; private set; }
        public string Nickname { get; private set; }
        public string Biography { get; private set; }
        public Uri? Avatar { get; private set; }
        public Uri? Header { get; private set; }
    }
}
