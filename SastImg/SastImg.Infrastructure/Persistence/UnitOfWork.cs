using SastImg.Domain.Repositories;

namespace SastImg.Infrastructure.Persistence
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly SastImgDbContext _dbContext;

        public UnitOfWork(SastImgDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
