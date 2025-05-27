using Primitives.Rule;
using SastImg.Domain.AlbumTagEntity;

namespace SastImg.Domain.AlbumAggregate.ImageEntity.Rules;

internal readonly struct ImageOwnNotMoreThan5TagsRule(ImageTagId[] tags) : IDomainRule
{
    public bool IsBroken => tags.Length > 5;

    public string Message => "Image couldn't contain more than 5 tags.";
}
