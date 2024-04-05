using FoxResult;
using Shared.Primitives.Query;

namespace Square.Application.TopicServices.Queries.GetTopic
{
    internal sealed class GetTopicQueryHandler(ITopicQueryRepository repository)
        : IQueryRequestHandler<GetTopicQuery, Result<TopicDetailedDto>>
    {
        private readonly ITopicQueryRepository _repository = repository;

        public async Task<Result<TopicDetailedDto>> Handle(
            GetTopicQuery request,
            CancellationToken cancellationToken
        )
        {
            var model = await _repository
                .GetTopicAsync(request.TopicId)
                .WaitAsync(cancellationToken);

            if (model == null)
            {
                return Result.Fail(Error.NotFound<TopicDetailedDto>());
            }

            var topic = TopicDetailedDto.MapFrom(model);

            return Result.Return(topic);
        }
    }
}
