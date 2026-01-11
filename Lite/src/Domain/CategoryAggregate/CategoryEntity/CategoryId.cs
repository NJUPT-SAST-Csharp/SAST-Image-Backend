using System.ComponentModel;
using Domain.Entity;
using Domain.Shared.Converter;

namespace Domain.CategoryAggregate.CategoryEntity;

[OpenJsonConverter<CategoryId>]
[TypeConverter(typeof(OpenTypeConverter<CategoryId>))]
public readonly record struct CategoryId(long Value) : ITypedId<CategoryId, long>
{
    public static CategoryId GenerateNew() => new(Snowflake.NewId);
}
