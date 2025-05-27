using Primitives.Rule;

namespace Account.Domain.UserEntity.Rules;

public readonly struct NicknameLengthRule(string nickname) : IDomainRule
{
    public const int MinLength = 1;
    public const int MaxLength = 12;

    public bool IsBroken { get; } = nickname.Length < MinLength || nickname.Length > MaxLength;

    public string Message { get; } =
        $"Nickname length should be between {MinLength} and {MaxLength}";
}
