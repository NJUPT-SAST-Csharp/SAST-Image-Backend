using Primitives.Rules;
using Shared.Primitives.DomainEvent;

namespace Shared.Primitives
{
    public abstract class Entity<T> : IEquatable<Entity<T>>, IDomainEventContainer
        where T : IEquatable<T>
    {
        protected Entity(T id)
        {
            Id = id;
        }

        public static bool operator ==(Entity<T> left, Entity<T> right) =>
            ReferenceEquals(left, right) || left is { } && right is { } && left.Equals(right);

        public static bool operator !=(Entity<T> left, Entity<T> right) => !(left == right);

        private readonly List<IDomainEvent> _domainEvents = [];

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public T Id { get; private init; }

        public bool Equals(Entity<T>? other)
        {
            if (other is null)
                return false;
            return other.Id.Equals(Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Entity<T>);
        }

        protected static void CheckRule(IDomainBusinessRule rule)
        {
            if (rule.IsBroken)
            {
                throw new DomainBusinessRuleInvalidException(rule);
            }
        }

        public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}
