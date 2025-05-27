using Identity;

namespace Account.Domain.UserEntity.Services;

public interface IUserRepository
{
    public Task<UserId> AddNewUserAsync(User user, CancellationToken cancellationToken = default);

    public Task<User> GetUserByIdAsync(UserId id, CancellationToken cancellationToken = default);

    public Task<User> GetUserByUsernameAsync(
        string username,
        CancellationToken cancellationToken = default
    );

    public Task<User> GetUserByEmailAsync(
        string email,
        CancellationToken cancellationToken = default
    );
}
