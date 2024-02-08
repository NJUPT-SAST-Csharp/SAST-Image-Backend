using Primitives.Rule;
using SastImg.Domain.TagEntity;

namespace SastImg.Domain.AlbumAggregate.ImageEntity.Rules
{
    internal readonly struct ImageOwnNotMoreThan5TagsRule(TagId[] tags) : IDomainBusinessRule
    {
        public bool IsBroken => tags.Length > 5;

        public string Message => "Image couldn't contain more than 5 tags.";
    }
}
