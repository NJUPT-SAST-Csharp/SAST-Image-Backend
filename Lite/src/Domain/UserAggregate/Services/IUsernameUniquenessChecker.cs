using Domain.UserAggregate.UserEntity;

namespace Domain.UserAggregate.Services;

public interface IUsernameUniquenessChecker
{
    public Task CheckAsync(Username username, CancellationToken cancellationToken = default);
}
