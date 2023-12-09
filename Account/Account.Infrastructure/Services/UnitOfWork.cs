using Account.Application.Services;
using Account.Infrastructure.Persistence;

namespace Account.Infrastructure.Services
{
    public sealed class UnitOfWork(AccountDbContext context) : IUnitOfWork
    {
        private readonly AccountDbContext _context = context;

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
