using Primitives.Entity;
using Utilities;

namespace Square.Domain.TopicAggregate.ColumnEntity
{
    public sealed class Column : EntityBase<ColumnId>
    {
        private Column()
            : base(default) { }

        internal Column(UserId authorId, string text, IEnumerable<TopicImage> images)
            : base(new(SnowFlakeIdGenerator.NewId))
        {
            _authorId = authorId;
            _text = text;
            _images = images;
        }

        #region Fields

        private readonly UserId _authorId;

        private readonly string _text = string.Empty;

        private readonly IEnumerable<TopicImage> _images = [];

        private readonly List<UserId> _likedBy = [];

        private readonly DateTime _uploadAt = DateTime.UtcNow;

        #endregion

        #region Properties

        public IEnumerable<TopicImage> Images => _images;

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
