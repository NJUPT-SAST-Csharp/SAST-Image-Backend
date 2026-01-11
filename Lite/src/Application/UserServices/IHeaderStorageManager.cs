using Domain.UserAggregate.UserEntity;

namespace Application.UserServices;

public interface IHeaderStorageManager
{
    public Task UpdateAsync(UserId user, Stream header, CancellationToken cancellationToken);
    public Stream? OpenReadStream(UserId user);
}
