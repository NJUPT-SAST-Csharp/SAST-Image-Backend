namespace Shared.Primitives
{
    public interface IAggregateRoot<T>
        where T : IAggregateRoot<T> { }
}
