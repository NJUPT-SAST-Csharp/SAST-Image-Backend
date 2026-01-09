using Account.Domain.UserEntity.ValueObjects;

namespace Account.Domain.UserEntity.Services;

public interface IUsernameUniquenessChecker
{
    public Task<bool> ExistsAsync(Username username, CancellationToken cancellationToken = default);
}
