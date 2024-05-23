using Primitives;
using Primitives.DomainEvent;
using Shared.Primitives.DomainEvent;

namespace Square.Infrastructure.Persistence
{
    internal class UnitOfWork(SquareDbContext domainContext, IDomainEventPublisher eventBus)
        : IUnitOfWork
    {
        private readonly SquareDbContext _domainContext = domainContext;
        private readonly IDomainEventPublisher _eventBus = eventBus;

        private bool isTransactionActive = false;

        public async Task CommitChangesAsync(CancellationToken cancellationToken = default)
        {
            if (isTransactionActive)
            {
                return;
            }

            await using var transaction = await _domainContext.Database.BeginTransactionAsync(
                cancellationToken
            );

            isTransactionActive = true;

            var domainEntities = _domainContext
                .ChangeTracker.Entries<IDomainEventContainer>()
                .Where(x => x.Entity.DomainEvents.Count > 0)
                .Select(x => x.Entity)
                .ToList();

            // First SaveChangesAsync() call is to save the changes made by the command handlers
            await _domainContext.SaveChangesAsync(cancellationToken);

            var domainEvents = domainEntities.SelectMany(x => x.DomainEvents);

            var tasks = domainEvents.Select(e => _eventBus.PublishAsync(e));

            // Where domain event handlers are executed
            await Task.WhenAll(tasks);

            domainEntities.ForEach(e => e.ClearDomainEvents());

            // Second SaveChangesAsync() call is to save the changes made by the domain event handlers
            await _domainContext.SaveChangesAsync(cancellationToken);

            // Commit the transaction
            await transaction.CommitAsync(cancellationToken);

            isTransactionActive = false;
        }
    }
}
