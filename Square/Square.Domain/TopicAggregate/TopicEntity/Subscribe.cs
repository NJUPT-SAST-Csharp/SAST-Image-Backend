namespace Square.Domain.TopicAggregate.TopicEntity
{
    public sealed record class Subscribe(UserId UserId, TopicId TopicId, DateTime SubscribedAt) { }
}
