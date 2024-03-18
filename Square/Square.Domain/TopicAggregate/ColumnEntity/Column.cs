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
            _uploadedAt = DateTime.UtcNow;
        }

        #region Fields

        private readonly UserId _authorId;

        private readonly string _text = string.Empty;

        private readonly IEnumerable<TopicImage> _images = [];

        private readonly List<Like> _likedBy = [];

        private readonly DateTime _uploadedAt;

        #endregion

        #region Properties

        public IEnumerable<TopicImage> Images => _images;

        #endregion

        #region Methods

        public void Liked(UserId userId)
        {
            _likedBy.Add(new(userId, Id, DateTime.UtcNow));
        }

        public void Unliked(UserId userId)
        {
            var like = _likedBy.FirstOrDefault(like => like.UserId == userId);

            if (like is null)
                return;

            _likedBy.Remove(like);
        }

        #endregion
    }
}
