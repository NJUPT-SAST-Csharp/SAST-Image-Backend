using SastImg.Domain.TagEntity;
using Shared.Primitives.Query;

namespace SastImg.Application.TagServices.GetTags
{
    public sealed class GetTagsQuery(long[] tagIds) : IQueryRequest<IEnumerable<TagDto>>
    {
        public TagId[] TagIds { get; } = Array.ConvertAll(tagIds, t => new TagId(t));
    }
}
