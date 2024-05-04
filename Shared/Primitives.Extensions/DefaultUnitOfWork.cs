using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Primitives.DomainEvent;
using Shared.Primitives.DomainEvent;

namespace Primitives.Extensions
{
    internal sealed class DefaultUnitOfWork<TDbContext>(
        TDbContext context,
        ILogger<IUnitOfWork> logger,
        IDomainEventPublisher publisher
    ) : IUnitOfWork
        where TDbContext : DbContext
    {
        private readonly IDomainEventPublisher _publisher = publisher;
        private readonly TDbContext _context = context;
        private readonly ILogger<IUnitOfWork> _logger = logger;

        private bool isTransactionStarted = false;

        public async Task CommitChangesAsync(CancellationToken cancellationToken = default)
        {
            if (isTransactionStarted)
            {
                return;
            }

            await using var transaction = await _context.Database.BeginTransactionAsync(
                cancellationToken
            );

            isTransactionStarted = true;

            UnitOfWorkLogger.LogBeginTransaction(_logger);

            var domainEntities = _context
                .ChangeTracker.Entries<IDomainEventContainer>()
                .Where(x => x.Entity.DomainEvents.Count > 0)
                .Select(x => x.Entity)
                .ToList();

            // First SaveChangesAsync() call is to save the changes made by the command handlers
            await _context.SaveChangesAsync(cancellationToken);

            var domainEvents = domainEntities.SelectMany(x => x.DomainEvents);

            var tasks = domainEvents.Select(e => _publisher.PublishAsync(e));

            // Where domain event handlers are executed
            await Task.WhenAll(tasks);

            domainEntities.ForEach(e => e.ClearDomainEvents());

            // Second SaveChangesAsync() call is to save the changes made by the domain event handlers
            await _context.SaveChangesAsync(cancellationToken);

            // Commit the transaction
            await transaction.CommitAsync(cancellationToken);

            UnitOfWorkLogger.LogEndTransaction(_logger);

            isTransactionStarted = false;
        }
    }

    internal sealed class DefaultUnitOfWork<TWriteContext, TQueryContext>(
        TWriteContext writeContext,
        TQueryContext queryContext,
        ILogger<IUnitOfWork> logger,
        IDomainEventPublisher publisher
    ) : IUnitOfWork
        where TWriteContext : DbContext
        where TQueryContext : DbContext
    {
        private readonly TWriteContext _writeContext = writeContext;
        private readonly TQueryContext _queryContext = queryContext;
        private readonly ILogger<IUnitOfWork> _logger = logger;
        private readonly IDomainEventPublisher _publisher = publisher;

        private bool isTransactionStarted = false;

        public async Task CommitChangesAsync(CancellationToken cancellationToken = default)
        {
            if (isTransactionStarted)
            {
                return;
            }

            await using var transaction = await _writeContext.Database.BeginTransactionAsync(
                cancellationToken
            );

            isTransactionStarted = true;

            UnitOfWorkLogger.LogBeginTransaction(_logger);

            var domainEntities = _writeContext
                .ChangeTracker.Entries<IDomainEventContainer>()
                .Where(x => x.Entity.DomainEvents.Count > 0)
                .Select(x => x.Entity)
                .ToList();

            // First SaveChangesAsync() call is to save the changes made by the command handlers
            await _writeContext.SaveChangesAsync(cancellationToken);

            var domainEvents = domainEntities.SelectMany(x => x.DomainEvents);

            var tasks = domainEvents.Select(e => _publisher.PublishAsync(e));

            // Where domain event handlers are executed
            await Task.WhenAll(tasks);

            domainEntities.ForEach(e => e.ClearDomainEvents());

            // Second SaveChangesAsync() call is to save the changes made by the domain event handlers
            await _writeContext.SaveChangesAsync(cancellationToken);
            await _queryContext.SaveChangesAsync(cancellationToken);

            // Commit the transaction
            await transaction.CommitAsync(cancellationToken);

            UnitOfWorkLogger.LogEndTransaction(_logger);

            isTransactionStarted = false;
        }
    }

    internal static partial class UnitOfWorkLogger
    {
        [LoggerMessage(LogLevel.Information, "Begin transaction.")]
        public static partial void LogBeginTransaction(ILogger logger);

        [LoggerMessage(LogLevel.Information, "End transaction.")]
        public static partial void LogEndTransaction(ILogger logger);
    }
}
