using FoxResult;
using Shared.Primitives.Query;
using Square.Domain.CategoryAggregate.CategoryEntity;

namespace Square.Application.TopicServices.Queries.GetTopics
{
    public sealed class GetTopicsQuery(int? categoryId, string? searchWord)
        : IQueryRequest<Result<IEnumerable<TopicDto>>>
    {
        public CategoryId? CategoryId { get; } = categoryId.HasValue ? new(categoryId.Value) : null;

        public string? SearchWord { get; } = searchWord;
    }
}
