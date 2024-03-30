using Shared.Primitives.DomainEvent;
using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Domain.TopicAggregate.Events
{
    public sealed class TopicInfoUpdatedEvent(
        TopicId topicId,
        TopicTitle title,
        TopicDescription description
    ) : IDomainEvent
    {
        public TopicId TopicId { get; } = topicId;
        public TopicTitle Title { get; } = title;
        public TopicDescription Description { get; } = description;
    }
}
