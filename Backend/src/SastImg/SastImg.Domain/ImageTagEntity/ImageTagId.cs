using System.ComponentModel;
using Primitives.Entity;
using Primitives.Utilities;

namespace SastImg.Domain.AlbumTagEntity;

[TypeConverter(typeof(OpenId<ImageTagId>))]
public readonly record struct ImageTagId : ITypedId<ImageTagId>
{
    public long Value { get; init; }

    public static ImageTagId GenerateNew() => new() { Value = SnowFlakeIdGenerator.NewId };

    public override string ToString()
    {
        return Value.ToString();
    }
}
