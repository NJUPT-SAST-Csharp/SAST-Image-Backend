using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Domain.ColumnAggregate.ColumnEntity
{
    public readonly record struct ColumnId((TopicId, UserId) Value) { }
}
