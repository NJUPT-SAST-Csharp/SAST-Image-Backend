using Primitives.Entity;
using Utilities;

namespace Square.Domain.TopicAggregate.ColumnEntity
{
    public sealed class Column : EntityBase<ColumnId>
    {
        private Column()
            : base(default) { }

        private Column(string text)
            : base(new(SnowFlakeIdGenerator.NewId))
        {
            _text = text;
        }

        internal static Column CreateNewColumn(string text)
        {
            Column column = new(text);
            return column;
        }

        #region Fields

        private readonly string _text = string.Empty;

        private readonly IReadOnlyCollection<TopicImage> _images = [];

        private readonly List<UserId> _likedBy = [];

        private readonly DateTime _uploadAt = DateTime.UtcNow;

        #endregion

        #region Methods

        public void Liked(UserId userId)
        {
            _likedBy.Add(userId);
        }

        public void Unliked(UserId userId)
        {
            _likedBy.Remove(userId);
        }

        #endregion
    }
}
