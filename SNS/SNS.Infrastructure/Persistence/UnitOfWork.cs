using SNS.Application.SeedWorks;

namespace SNS.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        public Task CommitChangesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
