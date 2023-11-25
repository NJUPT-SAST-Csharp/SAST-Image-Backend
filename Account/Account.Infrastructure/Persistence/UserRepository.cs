using Account.Application.Services;
using Account.Application.Users;

namespace Account.Infrastructure.Persistence
{
    internal sealed class UserRepository(IPasswordHasher passwordHasher, AccountDbContext dbContext)
        : IUserRepository
    {
        private readonly AccountDbContext _dbContext = dbContext;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;

        public Task CreateUserAsync(CancellationToken cancellationToken = default)
        {
            return _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task ModifyUserProfileAsync(CancellationToken cancellationToken = default)
        {
            return _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
