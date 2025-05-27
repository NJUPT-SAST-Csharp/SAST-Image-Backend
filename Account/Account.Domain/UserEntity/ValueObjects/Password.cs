using Utilities;

namespace Account.Domain.UserEntity.ValueObjects;

public sealed record class Password
{
    private Password(byte[] hash, byte[] salt)
    {
        _hash = hash;
        _salt = salt;
    }

    private readonly byte[] _hash;
    private readonly byte[] _salt;

    internal static Password NewPassword(string password)
    {
        byte[] salt = Argon2Hasher.RandomSalt;
        byte[] hash = Argon2Hasher.Hash(password, salt);

        return new Password(hash, salt);
    }

    internal async Task<bool> ValidateAsync(string password)
    {
        bool result = await Argon2Hasher.ValidateAsync(password, _hash, _salt);
        return result;
    }
}
