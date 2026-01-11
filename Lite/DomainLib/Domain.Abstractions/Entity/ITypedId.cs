namespace Domain.Entity;

public interface ITypedId<TId, TValue> : IEquatable<TId>, IBaseTypedId
    where TValue : IEquatable<TValue>
    where TId : ITypedId<TId, TValue>
{
    public TValue Value { get; init; }

    public static abstract TId GenerateNew();
}

public interface ITypedId<TId, TValue, TInput> : IEquatable<TId>, IBaseTypedId
    where TValue : IEquatable<TValue>
    where TId : ITypedId<TId, TValue>
{
    public static abstract TId GenerateNew(TInput requirement);
}

public interface IBaseTypedId
{
    public string ToString();
}
