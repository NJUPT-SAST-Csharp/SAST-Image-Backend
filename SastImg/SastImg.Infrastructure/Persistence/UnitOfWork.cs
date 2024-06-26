﻿using System.Data;
using Primitives;
using Primitives.DomainEvent;
using Shared.Primitives.DomainEvent;

namespace SastImg.Infrastructure.Persistence
{
    internal class UnitOfWork(SastImgDbContext dbContext, IDomainEventPublisher eventBus)
        : IUnitOfWork
    {
        private readonly SastImgDbContext _dbContext = dbContext;
        private readonly IDomainEventPublisher _eventBus = eventBus;

        private bool isTransactionBegin = false;

        public async Task CommitChangesAsync(CancellationToken cancellationToken = default)
        {
            if (isTransactionBegin)
            {
                return;
            }

            await using var transaction = await _dbContext.Database.BeginTransactionAsync(
                cancellationToken
            );

            isTransactionBegin = true;

            var domainEntities = _dbContext
                .ChangeTracker.Entries<IDomainEventContainer>()
                .Where(x => x.Entity.DomainEvents.Count > 0)
                .Select(x => x.Entity)
                .ToList();

            // First SaveChangesAsync() call is to save the changes made by the command handlers
            await _dbContext.SaveChangesAsync(cancellationToken);

            var domainEvents = domainEntities.SelectMany(x => x.DomainEvents);

            var tasks = domainEvents.Select(e => _eventBus.PublishAsync(e));

            // Where domain event handlers are executed
            await Task.WhenAll(tasks);

            domainEntities.ForEach(e => e.ClearDomainEvents());

            // Second SaveChangesAsync() call is to save the changes made by the domain event handlers
            await _dbContext.SaveChangesAsync(cancellationToken);

            // Commit the transaction
            await transaction.CommitAsync(cancellationToken);

            isTransactionBegin = false;
        }
    }
}
