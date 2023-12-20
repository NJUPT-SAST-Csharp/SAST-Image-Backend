using Account.Entity.UserEntity;
using Account.Entity.UserEntity.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Account.Infrastructure.Persistence
{
    public sealed class UserRepository(AccountDbContext dbContext, ILogger<UserRepository> logger)
        : IUserQueryRepository,
            IUserCheckRepository,
            IUserCommandRepository
    {
        private readonly AccountDbContext _dbContext = dbContext;
        private readonly ILogger<UserRepository> _logger = logger;

        public Task<bool> CheckEmailExistenceAsync(
            string email,
            CancellationToken cancellationToken = default
        )
        {
            return _dbContext
                .Users
                .Select(u => u.Email)
                .AnyAsync(e => e == email.ToUpperInvariant(), cancellationToken);
        }

        public Task<bool> CheckUsernameExistenceAsync(
            string username,
            CancellationToken cancellationToken = default
        )
        {
            return _dbContext
                .Users
                .Select(u => u.UsernameNormalized)
                .AnyAsync(name => name == username.ToUpperInvariant(), cancellationToken);
        }

        public async Task<bool> CreateUserAsync(
            User user,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                await _dbContext.Users.AddAsync(user, cancellationToken);
                var result = await _dbContext.SaveChangesAsync(cancellationToken);
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError("Add user failed for {exception}", ex.ToString());
                return false;
            }
        }

        public Task<User?> GetUserByEmailAsync(
            string email,
            CancellationToken cancellationToken = default
        )
        {
            return _dbContext
                .Users
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
            return _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        }

        public Task<User?> GetUserByUsernameAsync(
            string username,
            CancellationToken cancellationToken = default
        )
        {
            return _dbContext
                .Users
                .FirstOrDefaultAsync(
                    u => u.UsernameNormalized == username.ToUpperInvariant(),
                    cancellationToken
                );
        }
    }
}
