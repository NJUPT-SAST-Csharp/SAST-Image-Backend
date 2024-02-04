using SNS.Domain.UserEntity;
using SNS.Infrastructure.Persistence;

namespace SNS.Infrastructure.DomainRepositories
{
    internal class UserRepository(SNSDbContext context) : IUserRepository
    {
        private readonly SNSDbContext _context = context;

        public async Task<UserId> AddNewUserAsync(
            User user,
            CancellationToken cancellationToken = default
        )
        {
            var u = await _context.Users.AddAsync(user, cancellationToken);
            return u.Entity.Id;
        }
    }
}
