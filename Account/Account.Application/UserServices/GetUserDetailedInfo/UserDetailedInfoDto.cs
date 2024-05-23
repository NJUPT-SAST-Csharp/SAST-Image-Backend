namespace Account.Application.UserServices.GetUserDetailedInfo
{
    public sealed class UserDetailedInfoDto
    {
        public long Id { get; init; }
        public string Username { get; init; }
        public string Nickname { get; init; }
        public string Biography { get; init; }
        public DateOnly? Birthday { get; init; }
        public Uri? Website { get; init; }
    }
}
