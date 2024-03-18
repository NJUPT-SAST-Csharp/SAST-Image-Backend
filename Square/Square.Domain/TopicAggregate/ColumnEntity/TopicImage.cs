using Utilities;

namespace Square.Domain.TopicAggregate.ColumnEntity
{
    public sealed record class TopicImage(Uri Url)
    {
        public TopicImageId Id { get; } = new(SnowFlakeIdGenerator.NewId);
    }
}
