using Shared.Primitives.DomainEvent;
using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Domain.TopicAggregate.Events
{
    public sealed class TopicDeletedEvent(TopicId id) : IDomainEvent
    {
        public TopicId TopicId { get; } = id;
    }
}
