using Primitives;
using Primitives.DomainEvent;
using Shared.Primitives.DomainEvent;
using SNS.Infrastructure.Persistence;

namespace SNS.Infrastructure
{
    public class UnitOfWork(
        SNSDbContext context,
        IDomainEventContainer container,
        IDomainEventPublisher publisher
    ) : IUnitOfWork
    {
        private readonly SNSDbContext _context = context;
        private readonly IDomainEventContainer _container = container;
        private readonly IDomainEventPublisher _publisher = publisher;

        private bool _isTransactionStarted = false;

        public async Task CommitChangesAsync(CancellationToken cancellationToken = default)
        {
            if (_isTransactionStarted)
                return;

            await using var transaction = await _context.Database.BeginTransactionAsync(
                cancellationToken
            );

            _isTransactionStarted = true;

            await _context.SaveChangesAsync(cancellationToken);

            var tasks = _container.DomainEvents.Select(
                e => _publisher.PublishAsync(e, cancellationToken)
            );

            await Task.WhenAll(tasks).WaitAsync(cancellationToken);

            _container.ClearDomainEvents();

            await _context.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            _isTransactionStarted = false;
        }
    }
}
