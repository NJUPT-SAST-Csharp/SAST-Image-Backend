using Primitives.Entity;
using Square.Domain.ColumnAggregate.ColumnEntity;
using Utilities;

namespace Square.Domain.TopicAggregate.TopicEntity;

public sealed class Topic : EntityBase<TopicId>, ITopic
{
    private Topic()
        : base(default) { }

    private Topic(UserId authorId, TopicTitle title, TopicDescription description)
        : base(new(SnowFlakeIdGenerator.NewId))
    {
        _authorId = authorId;
        _title = title;
        _description = description;
    }

    internal static Topic CreateNewTopic(
        UserId authorId,
        TopicTitle title,
        TopicDescription description
    )
    {
        Topic topic = new(authorId, title, description);

        //TODO: Raise domain event
        return topic;
    }

    #region Fields

    private TopicTitle _title;

    private TopicDescription _description;

    private readonly UserId _authorId;

    private readonly DateTime _publishedAt = DateTime.UtcNow;

    private readonly List<TopicLike> _likes = [];

    private readonly List<TopicColumn> _columns = [];

    private readonly List<TopicSubscribe> _subscribers = [];

    private DateTime _updatedAt = DateTime.UtcNow;

    #endregion

    #region Properties

    public UserId AuthorId => _authorId;

    #endregion

    #region Methods

    public void AddColumn(UserId authorId, string? text, IEnumerable<ColumnImage> images)
    {
        Column column = new(authorId, Id, text, images);

        _columns.Add(column);

        _updatedAt = DateTime.UtcNow;
    }

    public void DeleteColumn(ColumnId columnId)
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

    public void UpdateInfo(TopicTitle title, TopicDescription description)
    {
        _title = title;
        _description = description;
    }

    public void ChangeToArchivedAlbum()
    {
        // TODO: Raise domain event
    }

    #endregion
}
