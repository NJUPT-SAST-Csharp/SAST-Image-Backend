using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Domain.TopicAggregate
{
    public interface ITopicRepository
    {
        public Task AddTopicAsync(ITopic topic);
        public Task DeleteTopicAsync(ITopic topic);
        public Task<ITopic?> GetTopicAsync(TopicId topicId);
    }
}
