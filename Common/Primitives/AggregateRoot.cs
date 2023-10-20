namespace Shared.Primitives
{
    public abstract class AggregateRoot<T> : Entity<T>
        where T : IEquatable<T>
    {
        protected AggregateRoot(T id = default!)
            : base(id) { }
    }
}
