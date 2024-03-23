using Square.Domain.TopicAggregate.TopicEntity;
using Utilities;

namespace Square.Domain.TopicAggregate.ColumnEntity
{
    public sealed record class TopicImage(Uri Url, Uri ThumbnailUrl)
    {
        private TopicId _topicId = default;
        public TopicImageId Id { get; } = new(SnowFlakeIdGenerator.NewId);

        internal void SetTopicId(TopicId topicId)
        {
            _topicId = topicId;
        }
    }
}
