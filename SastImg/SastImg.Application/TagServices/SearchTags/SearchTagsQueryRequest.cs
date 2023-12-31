using Shared.Primitives.Request;

namespace SastImg.Application.TagServices.SearchTags
{
    public sealed class SearchTagsQueryRequest(string name) : IQueryRequest<IEnumerable<TagDto>>
    {
        public string Name { get; } = name;
    }
}
