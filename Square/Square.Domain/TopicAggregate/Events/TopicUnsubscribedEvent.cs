using Shared.Primitives.DomainEvent;
using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Domain.TopicAggregate.Events
{
    public sealed class TopicUnsubscribedEvent(TopicId topicId, UserId userId) : IDomainEvent
    {
        public TopicId TopicId { get; } = topicId;

        public UserId UserId { get; } = userId;
    }
}
