using System.ComponentModel;
using Domain.Entity;
using Domain.Shared.Converter;

namespace Domain.AlbumAggregate.AlbumEntity;

[OpenJsonConverter<AlbumId>]
[TypeConverter(typeof(OpenTypeConverter<AlbumId>))]
public readonly record struct AlbumId(long Value) : ITypedId<AlbumId, long>
{
    public static AlbumId GenerateNew() => new(Snowflake.NewId);
}
