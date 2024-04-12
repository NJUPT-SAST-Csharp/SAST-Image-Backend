using System.Collections.Concurrent;
using Shared.Primitives.DomainEvent;

namespace SNS.Infrastructure
{
    internal sealed class DomainEventContainer : IDomainEventContainer
    {
        private readonly ConcurrentQueue<IDomainEvent> _queue = [];

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _queue.ToList();

        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            _queue.Enqueue(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _queue.Clear();
        }
    }
}
