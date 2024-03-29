using Primitives.Entity;
using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Domain.ColumnAggregate.ColumnEntity
{
    public sealed class Column : EntityBase<ColumnId>, IColumn
    {
        private Column()
            : base(default!) { }

        private Column(TopicId topicId, UserId authorId)
            : base(new(topicId, authorId)) { }

        internal static IColumn CreateNewColumn(TopicId topicId, UserId authorId)
        {
            return new Column(topicId, authorId);
        }

        #region Fields

        private readonly List<ColumnLike> _likes = [];

        #endregion

        #region Methods

        public void Like(UserId userId)
        {
            if (_likes.Any(like => like.UserId == userId))
            {
                return;
            }

            _likes.Add(new(Id, userId));
        }

        public void Unlike(UserId userId)
        {
            _likes.RemoveAll(x => x.UserId == userId);
        }

        public bool IsManagedBy(in RequesterInfo user)
        {
            return Id.AuthorId == user.Id || user.IsAdmin;
        }

        #endregion
    }
}
