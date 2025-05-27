using Primitives.Rule;

namespace Account.Domain.UserEntity.Rules;

public readonly struct BiographyValidRule(string biography) : IDomainRule
{
    public const int MaxLength = 100;

    public bool IsBroken { get; } = biography.Length > MaxLength;

    public string Message { get; } = $"Biography length should not be more than {MaxLength}";
}
