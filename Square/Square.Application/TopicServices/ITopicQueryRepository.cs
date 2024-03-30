using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Application.TopicServices
{
    public interface ITopicQueryRepository
    {
        public Task<TopicModel?> GetTopicAsync(TopicId id);

        public Task AddTopicAsync(TopicModel topic);

        public Task DeleteTopicAsync(TopicId id);
    }
}
