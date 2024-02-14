using Exceptions.Exceptions;
using Microsoft.EntityFrameworkCore;
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

        public async Task<User> GetUserAsync(
            UserId userId,
            CancellationToken cancellationToken = default
        )
        {
            var user = await _context
                .Users.Include("_following")
                .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
            if (user is null)
            {
                throw new DbNotFoundException(nameof(User), userId.Value.ToString());
            }
            return user;
        }
    }
}
