using Primitives.Entity;
using Primitives.Utilities;

namespace Identity;

public readonly record struct UserId : ITypedId<UserId>
{
    public UserId(long value)
    {
        Value = value;
    }

    public long Value { get; init; }

    public static UserId GenerateNew()
    {
        return new() { Value = SnowFlakeIdGenerator.NewId };
    }

    public override string ToString() => Value.ToString();
}
