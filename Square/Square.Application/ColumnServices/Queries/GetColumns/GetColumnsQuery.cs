using FoxResult;
using Shared.Primitives.Query;
using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Application.ColumnServices.Queries.GetColumns
{
    public sealed class GetColumnsQuery(long topicId)
        : IQueryRequest<Result<IEnumerable<ColumnDto>>>
    {
        public TopicId TopicId { get; } = new(topicId);
    }
}
