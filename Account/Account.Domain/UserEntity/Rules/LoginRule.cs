using Primitives.Rule;
using Utilities;

namespace Account.Domain.UserEntity.Rules
{
    internal readonly struct LoginRule(string password, byte[] passwordHash, byte[] passwordSalt)
        : IAsyncDomainBusinessRule
    {
        public async Task<bool> IsBrokenAsync()
        {
            return await Argon2Hasher.ValidateAsync(password, passwordHash, passwordSalt) == false;
        }

        public readonly string Message => "Password incorrect.";
    }
}
