using Square.Domain.TopicAggregate.ColumnEntity;

namespace Square.Domain.TopicAggregate
{
    public sealed record class Like(UserId UserId, ColumnId ColumnId, DateTime LikedAt) { }
}
