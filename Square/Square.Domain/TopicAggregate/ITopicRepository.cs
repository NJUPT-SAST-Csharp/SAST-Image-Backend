using Square.Domain.TopicAggregate.Commands;
using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Domain.TopicAggregate
{
    public interface ITopicRepository
    {
        public Task CreateTopic(CreateTopicCommand command);
        public Task DeleteTopic(DeleteTopicCommand command);

        public Task<ITopic> GetTopicAsync(TopicId topicId);
    }
}
