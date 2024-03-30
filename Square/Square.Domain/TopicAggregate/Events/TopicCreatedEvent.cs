using Shared.Primitives.DomainEvent;
using Square.Domain.TopicAggregate.Commands.CreateTopic;
using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Domain.TopicAggregate.Events
{
    public sealed class TopicCreatedEvent(CreateTopicCommand command, TopicId id) : IDomainEvent
    {
        public TopicId Id { get; } = id;
        public TopicTitle Title { get; } = command.Title;
        public TopicDescription Description { get; } = command.Description;
        public RequesterInfo Requester { get; } = command.Requester;
    }
}
