using Shared.Primitives.Query;

namespace SastImg.Application.TagServices.GetAllTags
{
    internal sealed class GetAllTagsQueryHandler(ITagQueryRepository repository)
        : IQueryRequestHandler<GetAllTagsQuery, IEnumerable<TagDto>>
    {
        private readonly ITagQueryRepository _repository = repository;

        public Task<IEnumerable<TagDto>> Handle(
            GetAllTagsQuery request,
            CancellationToken cancellationToken
        )
        {
            return _repository.GetAllTagsAsync(cancellationToken);
        }
    }
}
