using System.ComponentModel;
using Primitives.Entity;
using Primitives.Utilities;

namespace SastImg.Domain.CategoryEntity;

[TypeConverter(typeof(OpenId<CategoryId>))]
public readonly record struct CategoryId : ITypedId<CategoryId>
{
    public long Value { get; init; }

    public static CategoryId GenerateNew()
    {
        return new() { Value = SnowFlakeIdGenerator.NewId };
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}
