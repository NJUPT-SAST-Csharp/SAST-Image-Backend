using Common.Primitives;
using SastImg.Domain;

namespace SastImg.Infrastructure.Persistence
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly SastImgDbContext _dbContext;

        public UnitOfWork(SastImgDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<int> CommitChangesAsync(CancellationToken cancellationToken = default)
        {
            var domainEntities = _dbContext.ChangeTracker
                .Entries<IDomainEventContainer>()
                .Where(x => x.Entity.DomainEvents.Any())
                .Select(x => x.Entity)
                .ToList();

            var domainEvents = domainEntities.SelectMany(x => x.DomainEvents).ToList();

            // TODO: Pulish events.
            domainEvents.ForEach(e => { });

            domainEntities.ForEach(x => x.ClearDomainEvents());

            return _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
