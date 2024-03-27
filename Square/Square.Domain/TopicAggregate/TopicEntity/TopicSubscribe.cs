namespace Square.Domain.TopicAggregate.TopicEntity
{
    public sealed record class TopicSubscribe
    {
        public UserId UserId { get; }
        public TopicId TopicId { get; }

        internal TopicSubscribe(UserId userId, TopicId topicId)
        {
            UserId = userId;
            TopicId = topicId;
        }
    }
}
