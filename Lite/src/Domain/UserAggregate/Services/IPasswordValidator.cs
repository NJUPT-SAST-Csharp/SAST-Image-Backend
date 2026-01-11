using Domain.UserAggregate.UserEntity;

namespace Domain.UserAggregate.Services;

public interface IPasswordValidator
{
    public Task ValidateAsync(
        Password password,
        PasswordInput input,
        CancellationToken cancellationToken
    );
}
