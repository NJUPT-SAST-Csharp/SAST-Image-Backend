namespace Primitives.Entity;

public interface IAggregateRoot<T>
    where T : IAggregateRoot<T> { }
