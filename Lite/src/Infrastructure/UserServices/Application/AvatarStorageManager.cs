using Application.UserServices;
using Domain.UserAggregate.UserEntity;
using Infrastructure.Shared.Storage;
using Microsoft.Extensions.Options;

namespace Infrastructure.UserServices.Application;

internal sealed class AvatarStorageManager(
    ICompressProcessor processor,
    IOptions<StorageOptions> options
) : StorageManagerBase(options.Value.BasePath), IAvatarStorageManager
{
    private const string filename = "avatar.webp";

    public Stream? OpenReadStream(UserId user)
    {
        return OpenReadStream(user, filename);
    }

    public async Task UpdateAsync(UserId user, Stream avatar, CancellationToken cancellationToken)
    {
        var file = await processor.CompressAsync(avatar, 50, cancellationToken);

        await StoreAsync(file, user, filename, cancellationToken);
    }
}
