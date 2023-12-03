using Account.Entity.User;
using Account.Entity.User.Models;
using Account.Entity.User.Options;
using Account.Entity.User.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Account.Infrastructure.Persistence
{
    public sealed class UserRepository(AccountDbContext _dbContext) : IUserQueryRepository
    {
        public Task CreateUserAsync(
            CreateUserOptions options,
            CancellationToken cancellationToken = default
        )
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetUserByIdAsync(
            long userId,
            CancellationToken cancellationToken = default
        )
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetUserByUsernameAsync(
            string username,
            CancellationToken cancellationToken = default
        )
        {
            throw new NotImplementedException();
        }

        public Task<UserIdentity?> GetUserIdentityByIdAsync(
            long userId,
            CancellationToken cancellationToken = default
        )
        {
            throw new NotImplementedException();
        }

        public Task<UserIdentity?> GetUserIdentityByUsernameAsync(
            string username,
            CancellationToken cancellationToken = default
        )
        {
            return _dbContext
                .Users
                .Select(
                    u =>
                        new UserIdentity()
                        {
                            Id = u.Id,
                            Username = u.Username,
                            PasswordHash = u.PasswordHash
                        }
                )
                .FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
        }
    }
}
