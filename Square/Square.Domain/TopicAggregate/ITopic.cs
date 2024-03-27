using FoxResult;
using Square.Domain.ColumnAggregate.ColumnEntity;
using Square.Domain.TopicAggregate.TopicEntity.Commands.AddTopicColumn;
using Square.Domain.TopicAggregate.TopicEntity.Commands.DeleteTopicColumn;
using Square.Domain.TopicAggregate.TopicEntity.Commands.SubscribeTopic;
using Square.Domain.TopicAggregate.TopicEntity.Commands.UnsubscribeTopic;
using Square.Domain.TopicAggregate.TopicEntity.Commands.UpdateTopicInfo;

namespace Square.Domain.TopicAggregate
{
    public interface ITopic
    {
        public Result<ColumnId> AddColumn(AddTopicColumnCommand command);

        public Result DeleteColumn(DeleteTopicColumnCommand command);

        public Result UpdateTopicInfo(UpdateTopicInfoCommand command);

        public Result Subscribe(SubscribeTopicCommand command);

        public Result Unsubscribe(UnsubscribeTopicCommand command);
    }
}
