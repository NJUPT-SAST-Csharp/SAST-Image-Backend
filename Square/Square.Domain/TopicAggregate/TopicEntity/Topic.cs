using Primitives.Entity;
using Shared.Primitives;
using Square.Domain.TopicAggregate.ColumnEntity;

namespace Square.Domain.TopicAggregate.TopicEntity;

public sealed class Topic : EntityBase<TopicId>, IAggregateRoot<Topic>
{
    private Topic()
        : base(default) { }

    #region Fields

    private readonly DateTime _publishedAt = DateTime.UtcNow;

    private readonly string _title = string.Empty;

    private readonly Column _mainColumn;

    private readonly List<Column> _columns;

    private readonly List<UserId> _subscribers = [];

    private readonly List<UserId> _likedBy = [];

    #endregion

    #region Methods

    public void Reply(UserId authorId, string text, IEnumerable<Uri> imageUrls)
    {
        var images = imageUrls.Select(url => new TopicImage(url));
        Column column = new(authorId, text, images);

        _columns.Add(column);

        // TODO: Raise domain event
    }

    public void RemoveReply(ColumnId columnId)
    {
        var column = _columns.FirstOrDefault(column => column.Id == columnId);

        if (column is null)
            return;

        _columns.Remove(column);
    }

    public void ChangeToArchivedAlbum()
    {
        // TODO: Raise domain event
    }

    #endregion
}
