using Primitives.Rule;

namespace Account.Domain.UserEntity.Rules
{
    internal readonly struct UsernameValidRule : IDomainBusinessRule
    {
        public UsernameValidRule(string username)
        {
            if (username.Length < MinLength || username.Length > MaxLength)
            {
                IsBroken = true;
                Message = $"Username length should be between {MinLength} and {MaxLength}.";
                return;
            }
            else if (IsContainSpecialCharacters(username))
            {
                IsBroken = true;
                Message = "Username should not contain special characters.";
                return;
            }
            IsBroken = false;
        }

        public const int MinLength = 2;
        public const int MaxLength = 12;

        public readonly bool IsBroken { get; }

        public readonly string Message { get; }

        private static bool IsContainSpecialCharacters(string password)
        {
            return password.Any(c => (char.IsLetterOrDigit(c) || c == '_') == false);
        }
    }
}
