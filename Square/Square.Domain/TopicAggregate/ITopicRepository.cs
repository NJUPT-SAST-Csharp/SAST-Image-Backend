using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Domain.TopicAggregate
{
    public interface ITopicRepository
    {
        public Task<TopicId> AddTopicAsync(Topic topic);
    }
}
