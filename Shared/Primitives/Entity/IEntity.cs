using Shared.Primitives.DomainEvent;

namespace Primitives.Entity
{
    public interface IEntity<T>
        where T : IEquatable<T>
    {
        public T Id { get; }

        public IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

        public void ClearDomainEvents();
    }
}
