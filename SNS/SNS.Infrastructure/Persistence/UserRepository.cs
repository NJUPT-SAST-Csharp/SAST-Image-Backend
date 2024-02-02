using SNS.Domain.UserEntity;

namespace SNS.Infrastructure.Persistence
{
    internal class UserRepository(SNSDbContext context) : IUserRepository
    {
        private readonly SNSDbContext _context = context;
    }
}
