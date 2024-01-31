using Shared.Primitives.Query;

namespace SastImg.Application.TagServices.GetAllTags
{
    internal sealed class GetAllTagsQueryHandler
        : IQueryRequestHandler<GetAllTagsQuery, IEnumerable<TagDto>>
    {
        public Task<IEnumerable<TagDto>> Handle(
            GetAllTagsQuery request,
            CancellationToken cancellationToken
        )
        {
            throw new NotImplementedException();
        }
    }
}
