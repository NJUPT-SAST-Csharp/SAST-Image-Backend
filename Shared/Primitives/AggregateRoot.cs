using Shared.Primitives.DomainEvent;

namespace Shared.Primitives
{
    public abstract class AggregateRoot<T> : Entity<T>, IDomainEventContainer
        where T : IEquatable<T>
    {
        protected AggregateRoot(T id)
            : base(id) { }

        private readonly List<IDomainEvent> _domainEvents = new();

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}
