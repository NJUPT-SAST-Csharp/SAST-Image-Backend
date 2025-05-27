namespace Account.Application.UserServices.GetUserDetailedInfo;

public sealed class UserDetailedInfoDto
{
    public long Id { get; init; }
    public required string Username { get; init; }
    public required string Nickname { get; init; }
    public required string Biography { get; init; }
    public DateOnly? Birthday { get; init; }
    public Uri? Website { get; init; }
}
