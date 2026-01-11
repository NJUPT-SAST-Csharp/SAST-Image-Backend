using Domain.UserAggregate.UserEntity;

namespace Domain.UserAggregate.Services;

public interface IRegistryCodeChecker
{
    public Task CheckAsync(
        Username username,
        RegistryCode code,
        CancellationToken cancellationToken
    );
}
