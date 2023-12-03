using Account.Entity.User;
using Account.Entity.User.Models;
using Account.Entity.User.Options;

namespace Account.Entity.User.Repositories
{
    public interface IUserQueryRepository
    {
        public Task CreateUserAsync(
            CreateUserOptions options,
            CancellationToken cancellationToken = default
        );

        public Task<User?> GetUserByIdAsync(
            long userId,
            CancellationToken cancellationToken = default
        );

        public Task<User?> GetUserByUsernameAsync(
            string username,
            CancellationToken cancellationToken = default
        );
        public Task<UserIdentity?> GetUserIdentityByIdAsync(
            long userId,
            CancellationToken cancellationToken = default
        );
        public Task<UserIdentity?> GetUserIdentityByUsernameAsync(
            string username,
            CancellationToken cancellationToken = default
        );
    }
}
