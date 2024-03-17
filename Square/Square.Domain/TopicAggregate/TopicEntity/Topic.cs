using Primitives.Entity;
using Shared.Primitives;
using Square.Domain.TopicAggregate.ColumnEntity;

namespace Square.Domain.TopicAggregate.TopicEntity;

public sealed class Topic : EntityBase<TopicId>, IAggregateRoot<Topic>
{
    private Topic()
        : base(default) { }

    private readonly DateTime _publishedAt = DateTime.UtcNow;

    private readonly string _title = string.Empty;

    private readonly Column _topicColumn;

    private readonly List<UserId> _subscribers = [];

    private readonly List<UserId> _likedBy = [];
}
