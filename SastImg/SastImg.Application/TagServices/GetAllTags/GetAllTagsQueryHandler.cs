using Mediator;

namespace SastImg.Application.TagServices.GetAllTags;

public sealed class GetAllTagsQueryHandler(ITagQueryRepository repository)
    : IQueryHandler<GetAllTagsQuery, IEnumerable<TagDto>>
{
    public async ValueTask<IEnumerable<TagDto>> Handle(
        GetAllTagsQuery request,
        CancellationToken cancellationToken
    )
    {
        return await repository.GetAllTagsAsync(cancellationToken);
    }
}
