using Primitives.Rule;

namespace Account.Domain.UserEntity.Rules
{
    public readonly struct PasswordValidRule : IDomainBusinessRule
    {
        public PasswordValidRule(string password)
        {
            if (password.Length < MinLength || password.Length > MaxLength)
            {
                IsBroken = true;
                Message = $"Password length should be between {MinLength} and {MaxLength}.";
                return;
            }
            else if (IsContainSpecialCharacters(password))
            {
                IsBroken = true;
                Message = "Password should not contain special characters.";
                return;
            }
            IsBroken = false;
        }

        public const int MinLength = 6;
        public const int MaxLength = 20;

        public readonly bool IsBroken { get; }

        public readonly string Message { get; }

        private static bool IsContainSpecialCharacters(string password)
        {
            return password.Any(c => (char.IsLetterOrDigit(c) || c == '_') == false);
        }
    }
}
