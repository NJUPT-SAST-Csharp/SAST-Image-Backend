using Primitives.Rule;

namespace Account.Domain.UserEntity.Rules
{
    internal readonly struct BiographyValidRule(string biography) : IDomainBusinessRule
    {
        private const int MaxLength = 100;
        public bool IsBroken { get; } = biography.Length < MaxLength;

        public string Message { get; } = $"Biography length should not be more than {MaxLength}";
    }
}
