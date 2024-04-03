using FoxResult;
using Shared.Primitives.Query;

namespace Square.Application.TopicServices.Queries.GetTopics
{
    public sealed class GetTopicsQuery : IQueryRequest<Result<IEnumerable<TopicDto>>> { }
}
