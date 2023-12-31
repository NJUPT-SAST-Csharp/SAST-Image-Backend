using Shared.Primitives.Request;

namespace SastImg.Application.TagServices.GetTags
{
    internal class GetTagsQueryRequestHandler(ITagQueryRepository repository)
        : IQueryRequestHandler<GetTagsQueryRequest, IEnumerable<TagDto>>
    {
        private readonly ITagQueryRepository _repository = repository;

        public Task<IEnumerable<TagDto>> Handle(
            GetTagsQueryRequest request,
            CancellationToken cancellationToken
        )
        {
            return _repository.GetTagsAsync(request.TagIds, cancellationToken);
        }
    }
}
