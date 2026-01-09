using Primitives.Entity;

namespace SastImg.Domain.AlbumTagEntity;

public sealed class ImageTag : EntityBase<ImageTagId>
{
    private ImageTag(TagName name)
        : base(ImageTagId.GenerateNew())
    {
        _name = name;
    }

    private readonly TagName _name;

    public static ImageTag CreateNewTag(TagName name) => new(name);
}
