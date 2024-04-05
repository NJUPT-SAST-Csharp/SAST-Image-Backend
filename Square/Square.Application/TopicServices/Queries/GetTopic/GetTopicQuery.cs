using FoxResult;
using Shared.Primitives.Query;
using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Application.TopicServices.Queries.GetTopic
{
    public sealed class GetTopicQuery(long topicId) : IQueryRequest<Result<TopicDetailedDto>>
    {
        public TopicId TopicId { get; } = new(topicId);
    }
}
