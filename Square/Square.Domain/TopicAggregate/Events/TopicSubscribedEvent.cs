using Shared.Primitives.DomainEvent;
using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Domain.TopicAggregate.Events
{
    public sealed class TopicSubscribedEvent(TopicId topicId, UserId subscriberId) : IDomainEvent
    {
        public TopicId TopicId { get; } = topicId;
        public UserId SubscriberId { get; } = subscriberId;
    }
}
