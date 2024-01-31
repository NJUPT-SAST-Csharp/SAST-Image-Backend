using Shared.Primitives.Query;

namespace SastImg.Application.TagServices.SearchTags
{
    internal class SearchTagsQueryRequestHandler(ITagQueryRepository repository)
        : IQueryRequestHandler<SearchTagsQueryRequest, IEnumerable<TagDto>>
    {
        private readonly ITagQueryRepository _repository = repository;

        public Task<IEnumerable<TagDto>> Handle(
            SearchTagsQueryRequest request,
            CancellationToken cancellationToken
        )
        {
            return _repository.SearchTagsAsync(request.Name, cancellationToken);
        }
    }
}
