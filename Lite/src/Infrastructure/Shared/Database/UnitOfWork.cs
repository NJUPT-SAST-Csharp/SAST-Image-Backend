using Domain.Core.Event;
using Domain.Event;
using Domain.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Shared.Database;

internal sealed class UnitOfWork(
    DomainDbContext domainContext,
    QueryDbContext queryContext,
    ILogger<IUnitOfWork> logger,
    IDomainEventPublisher publisher
) : IUnitOfWork
{
    private readonly DomainDbContext _domainContext = domainContext;
    private readonly QueryDbContext _queryContext = queryContext;
    private readonly ILogger<IUnitOfWork> _logger = logger;
    private readonly IDomainEventPublisher _publisher = publisher;

    private bool _isTransactionStarted = false;

    public async Task CommitChangesAsync(CancellationToken cancellationToken = default)
    {
        if (_isTransactionStarted)
            return;

        await using var transaction = await _domainContext.Database.BeginTransactionAsync(
            cancellationToken
        );
        await using var _ = await _queryContext.Database.UseTransactionAsync(
            transaction.GetDbTransaction(),
            cancellationToken
        );

        try
        {
            _isTransactionStarted = true;

            var domainEntities = _domainContext
                .ChangeTracker.Entries<IDomainEventContainer>()
                .Where(x => x.Entity.DomainEvents.Count > 0)
                .Select(x => x.Entity)
                .ToList();

            await _domainContext.SaveChangesAsync(cancellationToken);

            var domainEvents = domainEntities.SelectMany(x => x.DomainEvents);
            var tasks = domainEvents.Select(e => _publisher.PublishAsync(e));
            await Task.WhenAll(tasks);
            domainEntities.ForEach(e => e.ClearDomainEvents());

            await _domainContext.SaveChangesAsync(cancellationToken);
            await _queryContext.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        finally
        {
            _isTransactionStarted = false;
        }
    }
}
