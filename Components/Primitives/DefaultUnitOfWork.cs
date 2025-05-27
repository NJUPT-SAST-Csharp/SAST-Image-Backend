using System.Diagnostics;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Primitives;

internal sealed class DefaultUnitOfWork<TDbContext>(
    TDbContext context,
    ILogger<IUnitOfWork> logger,
    IMediator publisher
) : IUnitOfWork
    where TDbContext : DbContext
{
    private bool isTransactionStarted = false;

    public async Task CommitChangesAsync(CancellationToken cancellationToken = default)
    {
        if (isTransactionStarted)
        {
            return;
        }

        await using var transaction = await context.Database.BeginTransactionAsync(
            cancellationToken
        );

        isTransactionStarted = true;
        string transactionId =
            Activity.Current?.TraceId.ToString() ?? Random.Shared.Next().ToString();
        int count = 0;

        UnitOfWorkLogger.LogBeginTransaction(logger, transactionId);

        var domainEntities = context
            .ChangeTracker.Entries<IDomainEventContainer>()
            .Where(x => x.Entity.DomainEvents.Count > 0)
            .Select(x => x.Entity)
            .ToList();

        // First SaveChangesAsync() call is to save the changes made by the command handlers
        count += await context.SaveChangesAsync(cancellationToken);

        var domainEvents = domainEntities.SelectMany(x => x.DomainEvents);

        foreach (var domainEvent in domainEvents)
        {
            await publisher.Publish(domainEvent, cancellationToken);
        }

        domainEntities.ForEach(e => e.ClearDomainEvents());

        // Second SaveChangesAsync() call is to save the changes made by the domain event handlers
        count += await context.SaveChangesAsync(cancellationToken);

        // Commit the transaction
        await transaction.CommitAsync(cancellationToken);

        UnitOfWorkLogger.LogEndTransaction(logger, transactionId, count);

        isTransactionStarted = false;
    }
}

internal sealed class DefaultUnitOfWork<TWriteContext, TQueryContext>(
    TWriteContext writeContext,
    TQueryContext queryContext,
    ILogger<IUnitOfWork> logger,
    IMediator publisher
) : IUnitOfWork
    where TWriteContext : DbContext
    where TQueryContext : DbContext
{
    private bool isTransactionStarted = false;

    public async Task CommitChangesAsync(CancellationToken cancellationToken = default)
    {
        if (isTransactionStarted)
        {
            return;
        }

        await using var transaction = await writeContext.Database.BeginTransactionAsync(
            cancellationToken
        );

        isTransactionStarted = true;
        string transactionId =
            Activity.Current?.TraceId.ToString() ?? Random.Shared.Next().ToString();
        int count = 0;

        UnitOfWorkLogger.LogBeginTransaction(logger, transactionId);

        var domainEntities = writeContext
            .ChangeTracker.Entries<IDomainEventContainer>()
            .Where(x => x.Entity.DomainEvents.Count > 0)
            .Select(x => x.Entity)
            .ToList();

        // First SaveChangesAsync() call is to save the changes made by the command handlers
        count += await writeContext.SaveChangesAsync(cancellationToken);

        var domainEvents = domainEntities.SelectMany(x => x.DomainEvents);

        foreach (var domainEvent in domainEvents)
        {
            await publisher.Publish(domainEvent, cancellationToken);
        }

        domainEntities.ForEach(e => e.ClearDomainEvents());

        // Second SaveChangesAsync() call is to save the changes made by the domain event handlers
        count += await writeContext.SaveChangesAsync(cancellationToken);
        count += await queryContext.SaveChangesAsync(cancellationToken);

        // Commit the transaction
        await transaction.CommitAsync(cancellationToken);

        UnitOfWorkLogger.LogEndTransaction(logger, transactionId, count);

        isTransactionStarted = false;
    }
}

internal static partial class UnitOfWorkLogger
{
    [LoggerMessage(
        LogLevel.Information,
        "[{Id}] Begin transaction.",
        EventName = "TransactionBegun"
    )]
    public static partial void LogBeginTransaction(ILogger logger, string id);

    [LoggerMessage(
        LogLevel.Information,
        "[{Id}] End transaction. {ChangeCount} records have been changed.",
        EventName = "TransactionEnded"
    )]
    public static partial void LogEndTransaction(ILogger logger, string id, int changeCount);
}
