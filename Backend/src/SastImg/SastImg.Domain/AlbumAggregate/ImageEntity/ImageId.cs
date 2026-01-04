using Primitives.Entity;
using Primitives.Utilities;

namespace SastImg.Domain.AlbumAggregate.ImageEntity;

public readonly record struct ImageId : ITypedId<ImageId>
{
    public long Value { get; init; }

    public static ImageId GenerateNew() => new() { Value = SnowFlakeIdGenerator.NewId };

    public override string ToString()
    {
        return Value.ToString();
    }
}
