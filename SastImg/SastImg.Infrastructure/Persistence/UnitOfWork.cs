using System.Data;
using Primitives.DomainEvent;
using SastImg.Domain;
using Shared.Primitives.DomainEvent;

namespace SastImg.Infrastructure.Persistence
{
    internal class UnitOfWork(SastImgDbContext dbContext, IDomainEventPublisher eventBus)
        : IUnitOfWork
    {
        private readonly SastImgDbContext _dbContext = dbContext;

        private readonly IDomainEventPublisher _eventBus = eventBus;

        public async Task<int> CommitChangesAsync(CancellationToken cancellationToken = default)
        {
            var domainEntities = _dbContext
                .ChangeTracker.Entries<IDomainEventContainer>()
                .Where(x => x.Entity.DomainEvents.Count > 0)
                .Select(x => x.Entity)
                .ToList();

            var domainEvents = domainEntities
                .SelectMany(x =>
                {
                    var events = x.DomainEvents;
                    x.ClearDomainEvents();
                    return events;
                })
                .ToList();

            var tasks = domainEvents.Select(e => _eventBus.PublishAsync(e));

            await Task.WhenAll(tasks);

            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
