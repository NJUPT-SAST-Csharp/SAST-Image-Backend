using Primitives.Rules;

namespace SastImg.Domain.AlbumAggregate.Rules
{
    internal sealed class ImageCannotOwnMoreThan5TagsRule(IEnumerable<long> tags)
        : IDomainBusinessRule
    {
        public bool IsBroken => tags.Count() > 5;

        public string Message => "Image couldn't contain more than 5 tags.";
    }
}
