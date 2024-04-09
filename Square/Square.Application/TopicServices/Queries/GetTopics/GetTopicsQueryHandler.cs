using FoxResult;
using Shared.Primitives.Query;

namespace Square.Application.TopicServices.Queries.GetTopics
{
    internal sealed class GetTopicsQueryHandler(ITopicQueryRepository repository)
        : IQueryRequestHandler<GetTopicsQuery, Result<IEnumerable<TopicDto>>>
    {
        private readonly ITopicQueryRepository _repository = repository;

        public async Task<Result<IEnumerable<TopicDto>>> Handle(
            GetTopicsQuery request,
            CancellationToken cancellationToken
        )
        {
            var models = await _repository
                .GetTopicsAsync(request.CategoryId)
                .WaitAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(request.SearchWord) == false)
            {
                models = models.Where(m => m.Title.Value.Contains(request.SearchWord));
            }

            var topics = models.Select(TopicDto.MapFrom);

            return Result.Return(topics);
        }
    }
}
