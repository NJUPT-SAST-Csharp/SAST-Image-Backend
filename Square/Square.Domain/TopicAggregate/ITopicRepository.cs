using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Domain.TopicAggregate
{
    public interface ITopicRepository
    {
        public Task<TopicId> AddTopicAsync(
            Topic topic,
            CancellationToken cancellationToken = default
        );

        public Task<Topic> GetTopicAsync(
            TopicId topicId,
            CancellationToken cancellationToken = default
        );

        public Task DeleteTopicAsync(Topic topic, CancellationToken cancellationToken = default);
    }
}
