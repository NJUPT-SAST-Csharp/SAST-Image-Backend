using SastImg.Domain.TagEntity;
using Shared.Primitives.Request;

namespace SastImg.Application.TagServices.GetTags
{
    public sealed class GetTagsQueryRequest(long[] tagIds) : IQueryRequest<IEnumerable<TagDto>>
    {
        public TagId[] TagIds { get; } = tagIds.Select(id => new TagId(id)).ToArray();
    }
}
