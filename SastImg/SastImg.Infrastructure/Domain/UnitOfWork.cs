using System.Data;
using Primitives.DomainEvent;
using SastImg.Domain;
using SastImg.Infrastructure.Persistence;
using Shared.Primitives.DomainEvent;

namespace SastImg.Infrastructure.Domain
{
    internal class UnitOfWork(SastImgDbContext dbContext, IDomainEventPublisher eventBus)
        : IUnitOfWork
    {
        private readonly SastImgDbContext _dbContext = dbContext;
        private readonly IDomainEventPublisher _eventBus = eventBus;

        public async Task CommitChangesAsync(CancellationToken cancellationToken = default)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync(
                cancellationToken
            );

            await _dbContext.SaveChangesAsync(cancellationToken);

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

            await _dbContext.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
    }
}
