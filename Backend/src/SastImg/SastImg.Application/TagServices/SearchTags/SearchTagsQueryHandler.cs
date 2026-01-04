using Mediator;

namespace SastImg.Application.TagServices.SearchTags;

public sealed class SearchTagsQueryHandler(ITagQueryRepository repository)
    : IQueryHandler<SearchTagsQuery, IEnumerable<TagDto>>
{
    public async ValueTask<IEnumerable<TagDto>> Handle(
        SearchTagsQuery request,
        CancellationToken cancellationToken
    )
    {
        return await repository.SearchTagsAsync(request.SearchName, cancellationToken);
    }
}
