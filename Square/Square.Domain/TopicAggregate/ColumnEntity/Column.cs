using Primitives.Entity;
using Square.Domain.TopicAggregate.TopicEntity;
using Utilities;

namespace Square.Domain.TopicAggregate.ColumnEntity
{
    public sealed class Column : EntityBase<ColumnId>
    {
        private Column()
            : base(default) { }

        internal Column(
            UserId authorId,
            TopicId topicId,
            string text,
            IEnumerable<TopicImage> images
        )
            : base(new(SnowFlakeIdGenerator.NewId))
        {
            _authorId = authorId;
            _topicId = topicId;
            _text = text;

            foreach (var image in images)
                image.SetTopicId(topicId);

            _images = images;
        }

        #region Fields

        private readonly TopicId _topicId;

        private readonly UserId _authorId;

        private readonly string _text = string.Empty;

        private readonly IEnumerable<TopicImage> _images = [];

        private readonly List<Like> _likes = [];

        private readonly DateTime _uploadedAt = DateTime.UtcNow;

        #endregion

        #region Properties

        public IEnumerable<TopicImage> Images => _images;

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
