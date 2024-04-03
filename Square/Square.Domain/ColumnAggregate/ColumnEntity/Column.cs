using FoxResult;
using Primitives.Entity;
using Square.Domain.ColumnAggregate.Commands.AddColumn;
using Square.Domain.ColumnAggregate.Commands.DeleteColumn;
using Square.Domain.ColumnAggregate.Commands.LikeColumn;
using Square.Domain.ColumnAggregate.Commands.UnlikeColumn;
using Square.Domain.ColumnAggregate.Events;
using Square.Domain.TopicAggregate;
using Square.Domain.TopicAggregate.TopicEntity;
using Utilities;

namespace Square.Domain.ColumnAggregate.ColumnEntity
{
    public sealed class Column : EntityBase<ColumnId>
    {
        private Column()
            : base(default!) { }

        private Column(TopicId topicId, UserId authorId)
            : base(new(SnowFlakeIdGenerator.NewId))
        {
            _topicId = topicId;
            _authorId = authorId;
        }

        internal static async Task<Result<ColumnId>> AddColumnAsync(
            AddColumnCommand command,
            ITopicRepository topics,
            IColumnRepository columns
        )
        {
            if (await topics.GetTopicAsync(command.TopicId) is null)
            {
                return Result.Fail(Error.NotFound<ColumnId>());
            }

            var column = await columns.GetColumnAsync(command.TopicId, command.Requester.Id);

            if (column is not null)
            {
                column.AddDomainEvent(new ExistingColumnUpdatedEvent(column.Id, command));
                return Result.Return(column.Id);
            }

            column = new(command.TopicId, command.Requester.Id);
            columns.AddColumn(column);

            column.AddDomainEvent(new NewColumnAddedEvent(column.Id, command));

            return Result.Return(column.Id);
        }

        #region Fields
        private readonly TopicId _topicId;

        private readonly UserId _authorId;

        private readonly HashSet<ColumnLike> _likes = [];

        #endregion

        #region Methods

        internal void Like(LikeColumnCommand command)
        {
            if (_likes.Any(like => like.UserId == command.Requester.Id))
            {
                return;
            }

            _likes.Add(new(Id, command.Requester.Id));

            AddDomainEvent(new ColumnLikedEvent(Id, command.Requester.Id));
        }

        internal void Unlike(UnlikeColumnCommand command)
        {
            int number = _likes.RemoveWhere(x => x.UserId == command.Requester.Id);

            if (number > 0)
            {
                AddDomainEvent(new ColumnUnlikedEvent(Id, command.Requester.Id));
            }
        }

        internal Result DeleteColumn(DeleteColumnCommand command, IColumnRepository repository)
        {
            if (command.Requester.Id != _authorId)
            {
                return Result.Fail(Error.Forbidden);
            }

            repository.DeleteColumn(this);

            AddDomainEvent(new ColumnDeletedEvent(Id));

            return Result.Success;
        }

        #endregion
    }
}
