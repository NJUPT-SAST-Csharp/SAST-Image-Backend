using Application.UserServices;
using Domain.UserAggregate.UserEntity;
using Infrastructure.Shared.Storage;
using Microsoft.Extensions.Options;

namespace Infrastructure.UserServices.Application;

internal sealed class HeaderStorageManager(
    ICompressProcessor processor,
    IOptions<StorageOptions> options
) : StorageManagerBase(options.Value.BasePath), IHeaderStorageManager
{
    private const string filename = "header.webp";

    public Stream? OpenReadStream(UserId user)
    {
        return OpenReadStream(user, filename);
    }

    public async Task UpdateAsync(UserId user, Stream header, CancellationToken cancellationToken)
    {
        var file = await processor.CompressAsync(header, 50, cancellationToken);
        await StoreAsync(file, user, filename, cancellationToken);
    }
}
