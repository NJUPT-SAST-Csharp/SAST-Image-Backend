namespace Square.Domain.TopicAggregate.TopicEntity
{
    public sealed record class TopicColumn
    {
        internal TopicColumn(TopicId topicId, UserId authorId)
        {
            TopicId = topicId;
            AuthorId = authorId;
        }

        public TopicId TopicId { get; }
        public UserId AuthorId { get; }
    }
}
