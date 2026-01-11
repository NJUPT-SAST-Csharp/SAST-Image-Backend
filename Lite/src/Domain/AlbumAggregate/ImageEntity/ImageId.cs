using System.ComponentModel;
using Domain.Entity;
using Domain.Shared.Converter;

namespace Domain.AlbumAggregate.ImageEntity;

[OpenJsonConverter<ImageId>]
[TypeConverter(typeof(OpenTypeConverter<ImageId>))]
public readonly record struct ImageId(long Value) : ITypedId<ImageId, long>
{
    public static ImageId GenerateNew() => new(Snowflake.NewId);
}
