using System.ComponentModel;
using Primitives.Entity;
using Primitives.Utilities;

namespace SastImg.Domain.AlbumAggregate.AlbumEntity;

[TypeConverter(typeof(OpenId<AlbumId>))]
public readonly record struct AlbumId : ITypedId<AlbumId>
{
    public long Value { get; init; }

    public static AlbumId GenerateNew() => new() { Value = SnowFlakeIdGenerator.NewId };

    public override string ToString() => Value.ToString();
}
