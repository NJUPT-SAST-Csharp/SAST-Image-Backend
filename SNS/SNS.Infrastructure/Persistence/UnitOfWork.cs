using SNS.Application.SeedWorks;

namespace SNS.Infrastructure.Persistence
{
    public class UnitOfWork(SNSDbContext context) : IUnitOfWork
    {
        private readonly SNSDbContext _context = context;

        public async Task CommitChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
