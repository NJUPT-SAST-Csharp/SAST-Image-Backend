using Domain.UserAggregate.Exceptions;
using Domain.UserAggregate.Services;
using Domain.UserAggregate.UserEntity;
using Microsoft.Extensions.Caching.Distributed;

namespace Infrastructure.UserServices.Domain;

internal sealed class RegistryCodeChecker(IDistributedCache cache) : IRegistryCodeChecker
{
    public async Task CheckAsync(
        Username username,
        RegistryCode code,
        CancellationToken cancellationToken
    )
    {
        _ =
            await cache.GetAsync(code.Value.ToString(), cancellationToken)
            ?? throw new RegistryCodeException();
    }
}
