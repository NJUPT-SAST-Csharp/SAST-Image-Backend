using Primitives.Rule;

namespace Account.Domain.UserEntity.Rules
{
    internal readonly struct NicknameLengthRule(string nickname) : IDomainBusinessRule
    {
        private const int MinLength = 1;
        private const int MaxLength = 12;

        public bool IsBroken { get; } = nickname.Length < MinLength || nickname.Length > MaxLength;

        public string Message { get; } =
            $"Nickname length should be between {MinLength} and {MaxLength}";
    }
}
