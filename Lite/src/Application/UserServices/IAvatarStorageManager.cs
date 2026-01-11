using Domain.UserAggregate.UserEntity;

namespace Application.UserServices;

public interface IAvatarStorageManager
{
    public Task UpdateAsync(UserId user, Stream avatar, CancellationToken cancellationToken);
    public Stream? OpenReadStream(UserId user);
}
