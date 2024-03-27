using Shared.Primitives.DomainEvent;
using Square.Domain.ColumnAggregate.ColumnEntity;

namespace Square.Domain.TopicAggregate.TopicEntity.Commands.AddTopicColumn
{
    public sealed class TopicColumnAddedDomainEvent(ColumnId columnId, TopicId topicId)
        : IDomainEvent
    {
        public ColumnId ColumnId { get; } = columnId;
        public TopicId TopicId { get; } = topicId;
    }
}
