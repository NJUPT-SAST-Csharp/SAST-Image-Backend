namespace Primitives.Entity;

public interface ITypedId<TId, TValue> : IEquatable<TId>
    where TValue : IEquatable<TValue>
    where TId : ITypedId<TId, TValue>
{
    public TValue Value { get; init; }

    public static abstract TId GenerateNew();
}

public interface ITypedId<TId> : IEquatable<TId>, ITypedId<TId, long>
    where TId : ITypedId<TId, long> { }
