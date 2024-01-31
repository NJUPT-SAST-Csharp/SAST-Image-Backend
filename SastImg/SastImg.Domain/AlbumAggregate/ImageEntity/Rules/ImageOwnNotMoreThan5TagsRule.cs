using Primitives.Rule;

namespace SastImg.Domain.AlbumAggregate.ImageEntity.Rules
{
    internal sealed class ImageOwnNotMoreThan5TagsRule(long[] tags) : IDomainBusinessRule
    {
        public bool IsBroken => tags.Length > 5;

        public string Message => "Image couldn't contain more than 5 tags.";
    }
}
