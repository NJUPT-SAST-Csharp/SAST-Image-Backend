namespace Square.Domain.TopicAggregate
{
    public sealed record class Like(UserId UserId, DateTime LikedAt) { }
}
