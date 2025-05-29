using System.ComponentModel;
using Primitives.Entity;
using Primitives.Utilities;

namespace Identity;

[OpenJsonConverter<UserId>]
[TypeConverter(typeof(OpenId<UserId>))]
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
