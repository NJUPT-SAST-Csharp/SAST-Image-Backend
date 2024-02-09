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
            var check = await _context.Users.FindAsync([user.Id], cancellationToken);

            if (check is not null)
                return check.Id;

            var newUser = await _context.Users.AddAsync(user, cancellationToken);
            return newUser.Entity.Id;
        }
    }
}
