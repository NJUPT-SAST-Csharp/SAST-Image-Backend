using Account.Domain.UserEntity.Services;
using Account.Domain.UserEntity.ValueObjects;
using Utilities;

namespace Account.Infrastructure.DomainServices;

internal sealed class PasswordProcessor : IPasswordGenerator, IPasswordValidator
{
    public async ValueTask<Password> GenerateAsync(
        PasswordInput password,
        CancellationToken cancellationToken = default
    )
    {
        byte[] salt = Argon2Hasher.RandomSalt;
        byte[] hash = await Argon2Hasher
            .HashAsync(password.Value, salt)
            .WaitAsync(cancellationToken);

        return new() { Hash = hash, Salt = salt };
    }

    public async ValueTask<bool> ValidateAsync(
        PasswordInput input,
        Password password,
        CancellationToken cancellationToken = default
    )
    {
        return await Argon2Hasher.ValidateAsync(input.Value, password.Hash, password.Salt);
    }
}
