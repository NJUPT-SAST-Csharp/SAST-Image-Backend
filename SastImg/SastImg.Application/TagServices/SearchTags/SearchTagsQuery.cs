using Shared.Primitives.Query;

namespace SastImg.Application.TagServices.SearchTags
{
    public sealed class SearchTagsQuery(string name) : IQueryRequest<IEnumerable<TagDto>>
    {
        public string Name { get; } = name;
    }
}
