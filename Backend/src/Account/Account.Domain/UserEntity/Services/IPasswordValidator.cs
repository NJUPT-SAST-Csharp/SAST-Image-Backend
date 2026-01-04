using Account.Domain.UserEntity.ValueObjects;

namespace Account.Domain.UserEntity.Services;

public interface IPasswordValidator
{
    public ValueTask<bool> ValidateAsync(
        PasswordInput input,
        Password password,
        CancellationToken cancellationToken = default
    );
}
