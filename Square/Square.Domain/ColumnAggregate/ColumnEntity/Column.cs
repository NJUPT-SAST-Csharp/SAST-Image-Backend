using Primitives.Entity;
using Square.Domain.TopicAggregate.TopicEntity;
using Utilities;

namespace Square.Domain.ColumnAggregate.ColumnEntity
{
    public sealed class Column : EntityBase<ColumnId>
    {
        private Column()
            : base(default) { }

        internal Column(
            UserId authorId,
            TopicId topicId,
            ColumnText text,
            IEnumerable<ColumnImage> images
        )
            : base(new(SnowFlakeIdGenerator.NewId))
        {
            _authorId = authorId;
            _topicId = topicId;
            _text = text;

            foreach (var image in images)
                image.SetTopicId(topicId);

            _images = images.ToList();
        }

        #region Fields

        private readonly TopicId _topicId;

        private readonly UserId _authorId;

        private readonly ColumnText _text;

        private readonly List<ColumnImage> _images = [];

        private readonly List<TopicLike> _likes = [];

        private readonly DateTime _uploadedAt = DateTime.UtcNow;

        #endregion

        #region Methods

        public void Liked(UserId userId)
        {
            if (_likes.Any(like => like.UserId == userId))
                return;

            _likes.Add(new(userId, DateTime.UtcNow));
        }

        public void Unliked(UserId userId)
        {
            var like = _likes.FirstOrDefault(like => like.UserId == userId);

            if (like is null)
                return;

            _likes.Remove(like);
        }

        #endregion
    }
}
