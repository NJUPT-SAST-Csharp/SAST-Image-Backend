using Account.Entity.UserEntity;
using Account.Entity.UserEntity.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Account.Infrastructure.Persistence
{
    public sealed class UserRepository(AccountDbContext dbContext)
        : IUserQueryRepository,
            IUserCheckRepository,
            IUserCommandRepository
    {
        private readonly AccountDbContext _dbContext = dbContext;

        public Task<bool> CheckEmailExistenceAsync(
            string email,
            CancellationToken cancellationToken = default
        )
        {
            return _dbContext
                .Users.Select(u => u.Email)
                .AnyAsync(e => e == email.ToUpperInvariant(), cancellationToken);
        }

        public Task<bool> CheckUsernameExistenceAsync(
            string username,
            CancellationToken cancellationToken = default
        )
        {
            return _dbContext
                .Users.Select(u => u.UsernameNormalized)
                .AnyAsync(name => name == username.ToUpperInvariant(), cancellationToken);
        }

        public async Task CreateUserAsync(User user, CancellationToken cancellationToken = default)
        {
            user.AddRole(
                await _dbContext.Roles.SingleAsync(r => r.Name == "User", cancellationToken)
            );
            await _dbContext.Users.AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task<User?> GetUserByEmailAsync(
            string email,
            CancellationToken cancellationToken = default
        )
        {
            return _dbContext
                .Users.Include(u => u.Roles)
                .FirstOrDefaultAsync(
                    user => user.Email == email.ToUpperInvariant(),
                    cancellationToken
                );
        }

        public Task<User?> GetUserByIdAsync(
            long userId,
            CancellationToken cancellationToken = default
        )
        {
            return _dbContext
                .Users.Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        }

        public Task<User?> GetUserByUsernameAsync(
            string username,
            CancellationToken cancellationToken = default
        )
        {
            return _dbContext
                .Users.Include(u => u.Roles)
                .FirstOrDefaultAsync(
                    u => u.UsernameNormalized == username.ToUpperInvariant(),
                    cancellationToken
                );
        }
    }
}
