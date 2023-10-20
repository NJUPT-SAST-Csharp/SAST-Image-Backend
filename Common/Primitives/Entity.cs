namespace Shared.Primitives
{
    public abstract class Entity<T> : IEquatable<Entity<T>>
        where T : IEquatable<T>
    {
        protected Entity(T id = default!)
        {
            Id = id;
        }

        public static bool operator ==(Entity<T> left, Entity<T> right) =>
            ReferenceEquals(left, right) || (left is { } && right is { } && left.Equals(right));

        public static bool operator !=(Entity<T> left, Entity<T> right) => !(left == right);

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
    }
}
