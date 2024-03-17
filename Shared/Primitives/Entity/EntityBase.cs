using Primitives.Rule;
using Shared.Primitives.DomainEvent;

namespace Primitives.Entity;

public abstract class EntityBase<T> : IEquatable<EntityBase<T>>, IDomainEventContainer, IEntity<T>
    where T : IEquatable<T>
{
    protected EntityBase(T id)
    {
        Id = id;
    }

    public static bool operator ==(EntityBase<T> left, EntityBase<T> right) =>
        ReferenceEquals(left, right) || left is { } && right is { } && left.Equals(right);

    public static bool operator !=(EntityBase<T> left, EntityBase<T> right) => !(left == right);

    private readonly List<IDomainEvent> _domainEvents = [];

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public T Id { get; private init; }

    public bool Equals(EntityBase<T>? other)
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
        return Equals(obj as EntityBase<T>);
    }

    protected static void CheckRule<TRule>(in TRule rule)
        where TRule : IDomainBusinessRule
    {
        if (rule.IsBroken)
        {
            throw new DomainBusinessRuleInvalidException(rule, typeof(TRule).Name);
        }
    }

    protected static async Task CheckRuleAsync<TAsyncRule>(TAsyncRule rule)
        where TAsyncRule : IAsyncDomainBusinessRule
    {
        if (await rule.IsBrokenAsync())
        {
            throw new DomainBusinessRuleInvalidException(rule, typeof(TAsyncRule).Name);
        }
    }

    public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();
}
