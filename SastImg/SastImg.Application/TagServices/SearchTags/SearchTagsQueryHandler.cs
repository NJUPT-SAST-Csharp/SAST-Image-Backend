using Shared.Primitives.Query;

namespace SastImg.Application.TagServices.SearchTags
{
    internal class SearchTagsQueryHandler(ITagQueryRepository repository)
        : IQueryRequestHandler<SearchTagsQuery, IEnumerable<TagDto>>
    {
        private readonly ITagQueryRepository _repository = repository;

        public Task<IEnumerable<TagDto>> Handle(
            SearchTagsQuery request,
            CancellationToken cancellationToken
        )
        {
            return _repository.SearchTagsAsync(request.Name, cancellationToken);
        }
    }
}
