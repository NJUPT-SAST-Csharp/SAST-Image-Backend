using Domain.UserAggregate.UserEntity;

namespace Domain.UserAggregate.Services;

public interface IPasswordGenerator
{
    public Task<Password> GenerateAsync(
        PasswordInput password,
        CancellationToken cancellationToken
    );
}
