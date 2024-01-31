using Shared.Primitives.Query;

namespace SastImg.Application.TagServices.GetTags
{
    internal class GetTagsQueryHandler(ITagQueryRepository repository)
        : IQueryRequestHandler<GetTagsQuery, IEnumerable<TagDto>>
    {
        private readonly ITagQueryRepository _repository = repository;

        public Task<IEnumerable<TagDto>> Handle(
            GetTagsQuery request,
            CancellationToken cancellationToken
        )
        {
            return _repository.GetTagsAsync(request.TagIds, cancellationToken);
        }
    }
}
