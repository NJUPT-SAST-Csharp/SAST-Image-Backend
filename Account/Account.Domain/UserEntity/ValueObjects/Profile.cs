namespace Account.Domain.UserEntity.ValueObjects
{
    public sealed record class Profile(
        string Nickname,
        string Biography,
        DateOnly? Birthday,
        Uri? Website
    )
    {
        internal static Profile Default => new("SASTer", string.Empty, null, null);
    }
}
