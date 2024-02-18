using System.Text.RegularExpressions;
using Primitives.Rule;

namespace Account.Domain.UserEntity.Rules
{
    public readonly struct EmailValidRule(string email) : IDomainBusinessRule
    {
        public const int MaxLength = 50;

        public bool IsBroken { get; } = IsValidEmail(email) == false;

        public string Message => "Invalid email.";

        private static bool IsValidEmail(string email)
        {
            if (email.Length > MaxLength)
                return false;

            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            return Regex.IsMatch(email, pattern);
        }
    }
}
