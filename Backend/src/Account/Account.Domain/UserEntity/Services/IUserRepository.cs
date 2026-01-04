using Account.Domain.UserEntity.ValueObjects;
using Identity;

namespace Account.Domain.UserEntity.Services;

public interface IUserRepository
{
    public Task<UserId> AddAsync(User user, CancellationToken cancellationToken = default);

    public Task<User> GetByIdAsync(UserId id, CancellationToken cancellationToken = default);

    public Task<User> GetByUsernameAsync(
        Username username,
        CancellationToken cancellationToken = default
    );
}
