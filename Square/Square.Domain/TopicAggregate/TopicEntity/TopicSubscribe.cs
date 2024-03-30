namespace Square.Domain.TopicAggregate.TopicEntity
{
    public sealed record class TopicSubscribe
    {
        public UserId UserId { get; }
        public TopicId TopicId { get; }

        public TopicSubscribe(UserId userId, TopicId topicId)
        {
            UserId = userId;
            TopicId = topicId;
        }
    }
}
