using FoxResult;
using Shared.Primitives.DomainEvent;
using Square.Domain.TopicAggregate.Commands.UpdateTopicInfo;
using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Domain.TopicAggregate
{
    public interface ITopic : IDomainEventContainer
    {
        public TopicId Id { get; }

        public Result UpdateTopicInfo(UpdateTopicInfoCommand command);

        public void Subscribe(UserId subscriberId);

        public void Unsubscribe(UserId subscriberId);
    }
}
