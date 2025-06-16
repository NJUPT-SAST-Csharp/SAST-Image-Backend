using Account.Domain.UserEntity.ValueObjects;

namespace Account.Domain.UserEntity.Services;

public interface IPasswordGenerator
{
    public ValueTask<Password> GenerateAsync(
        PasswordInput password,
        CancellationToken cancellationToken = default
    );
}
