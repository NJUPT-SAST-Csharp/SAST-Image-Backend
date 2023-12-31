using Shared.Primitives.Request;

namespace SastImg.Application.TagServices.GetAllTags
{
    public sealed class GetAllTagsQueryRequest : IQueryRequest<IEnumerable<TagDto>> { }
}
