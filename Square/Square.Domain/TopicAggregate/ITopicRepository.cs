using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Domain.TopicAggregate
{
    public interface ITopicRepository
    {
        public void AddTopic(Topic topic);
        public void DeleteTopic(Topic topic);
        public Task<Topic?> GetTopicAsync(TopicId topicId);
    }
}
