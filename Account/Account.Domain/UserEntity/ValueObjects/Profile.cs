namespace Account.Domain.UserEntity.ValueObjects
{
    public sealed record class Profile(
        string Nickname,
        string Biography,
        DateOnly? Birthday,
        Uri? Website
    ) { }
}
