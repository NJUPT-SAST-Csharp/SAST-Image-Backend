using Account.Entity.User;
using Account.Entity.User.Repositories;
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
                .AnyAsync(e => e == email, cancellationToken);
        }

        public Task<bool> CheckSignInAsync(
            string username,
            byte[] passwordHash,
            CancellationToken cancellationToken = default
        )
        {
            return _dbContext
                .Users
                .Select(u => new { u.Username, u.PasswordHash })
                .AnyAsync(
                    u => u.Username == username && u.PasswordHash == passwordHash,
                    cancellationToken: cancellationToken
                );
        }

        public Task<bool> CheckUsernameExistenceAsync(
            string username,
            CancellationToken cancellationToken = default
        )
        {
            return _dbContext
                .Users
                .Select(u => u.Username)
                .AnyAsync(name => name == username, cancellationToken);
        }

        public async Task<bool> CreateUserAsync(
            User user,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                await _dbContext.Users.AddAsync(user, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Add user failed for {exception}", ex.ToString());
                return false;
            }
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
                .FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
        }
    }
}
