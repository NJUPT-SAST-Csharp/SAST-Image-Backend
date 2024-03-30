using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Domain.TopicAggregate
{
    public interface ITopicUniquenessChecker
    {
        public Task<bool> IsConflictAsync(TopicTitle title);
    }
}
