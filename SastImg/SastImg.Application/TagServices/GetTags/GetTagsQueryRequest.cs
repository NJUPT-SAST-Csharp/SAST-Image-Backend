using Shared.Primitives.Request;

namespace SastImg.Application.TagServices.GetTags
{
    public sealed class GetTagsQueryRequest(long[] tagIds) : IQueryRequest<IEnumerable<TagDto>>
    {
        public long[] TagIds { get; } = tagIds;
    }
}
