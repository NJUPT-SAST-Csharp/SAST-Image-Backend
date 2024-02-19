namespace Account.Application.UserServices.GetUserBriefInfo
{
    public sealed class UserBriefInfoDto
    {
        public string Username { get; init; }
        public string Nickname { get; init; }
        public Uri? Avatar { get; init; }
    }
}
