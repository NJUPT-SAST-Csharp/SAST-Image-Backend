using Primitives.Entity;
using Shared.Primitives;
using Square.Domain.TopicAggregate.ColumnEntity;
using Utilities;

namespace Square.Domain.TopicAggregate.TopicEntity;

public sealed class Topic : EntityBase<TopicId>, IAggregateRoot<Topic>
{
    private Topic()
        : base(default) { }

    private Topic(UserId authorId, string title, string description)
        : base(new(SnowFlakeIdGenerator.NewId))
    {
        _authorId = authorId;
        _title = title;
        _description = description;
    }

    public static Topic CreateNewTopic(UserId authorId, string title, string description)
    {
        //TODO: Check
        Topic topic = new(authorId, title, description);

        //TODO: Raise domain event
        return topic;
    }

    #region Fields

    private readonly string _title = string.Empty;

    private readonly string _description = string.Empty;

    private readonly UserId _authorId;

    private readonly DateTime _publishedAt = DateTime.UtcNow;

    private readonly List<Like> _likes = [];

    private readonly List<Column> _columns = [];

    private readonly List<Subscribe> _subscribers = [];

    private DateTime _updatedAt = DateTime.UtcNow;

    #endregion

    #region Methods

    public void AddColumn(UserId authorId, string text, IEnumerable<TopicImage> images)
    {
        Column column = new(authorId, Id, text, images);

        _columns.Add(column);

        _updatedAt = DateTime.UtcNow;
    }

    public void Reply(UserId authorId, string text, IEnumerable<TopicImage> images)
    {
        Column column = new(authorId, Id, text, images);

        _columns.Add(column);

        _updatedAt = DateTime.UtcNow;

        // TODO: Raise domain event
    }

    public void RemoveReply(ColumnId columnId)
    {
        var column = _columns.FirstOrDefault(column => column.Id == columnId);

        if (column is null)
            return;

        _columns.Remove(column);
    }

    public void Subscribe(UserId userId)
    {
        if (_subscribers.Any(subscriber => subscriber.UserId == userId))
            return;

        _subscribers.Add(new(userId, Id, DateTime.UtcNow));
    }

    public void Unsubscribe(UserId userId)
    {
        var subscribe = _subscribers.FirstOrDefault(subscribe => subscribe.UserId == userId);

        if (subscribe is null)
            return;

        _subscribers.Remove(subscribe);
    }

    public void ChangeToArchivedAlbum()
    {
        // TODO: Raise domain event
    }

    #endregion
}
