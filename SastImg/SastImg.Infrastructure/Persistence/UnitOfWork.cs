using SastImg.Application.Services.EventBus;
using SastImg.Domain;
using Shared.Primitives.DomainEvent;

namespace SastImg.Infrastructure.Persistence
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly SastImgDbContext _dbContext;

        private readonly IInternalEventBus _eventBus;

        public UnitOfWork(SastImgDbContext dbContext, IInternalEventBus eventBus)
        {
            _dbContext = dbContext;
            _eventBus = eventBus;
        }

        public async Task<int> CommitChangesAsync(CancellationToken cancellationToken = default)
        {
            var domainEntities = _dbContext
                .ChangeTracker
                .Entries<IDomainEventContainer>()
                .Where(x => x.Entity.DomainEvents.Any())
                .Select(x => x.Entity)
                .ToList();

            var domainEvents = domainEntities.SelectMany(x => x.DomainEvents).ToList();

            var tasks = domainEvents.Select(e => _eventBus.PublishAsync(e));

            await Task.WhenAll(tasks);

            domainEntities.ForEach(x => x.ClearDomainEvents());

            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
