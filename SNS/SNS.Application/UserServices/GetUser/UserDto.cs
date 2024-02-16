namespace SNS.Application.UserServices.GetUser
{
    public sealed class UserDto
    {
        public string Nickname { get; init; }
        public string Biography { get; init; }
        public Uri? Avatar { get; init; }
        public Uri? Header { get; init; }
    }
}
