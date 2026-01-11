namespace Domain.Entity;

public interface IEntity<T> : IBaseEntity
    where T : IEquatable<T>
{
    public T Id { get; }
}

public interface IBaseEntity { }
