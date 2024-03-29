using FoxResult;
using Primitives;
using Primitives.Command;
using Square.Domain.ColumnAggregate;
using Square.Domain.ColumnAggregate.ColumnEntity;
using Square.Domain.TopicAggregate;

namespace Square.Domain.DomainServices.AddColumnToTopic
{
    internal sealed class AddColumnToTopicCommandHandler(
        ITopicRepository topics,
        IColumnRepository columns,
        IUnitOfWork unitOfWork
    ) : ICommandRequestHandler<AddColumnToTopicCommand, Result<ColumnId>>
    {
        private readonly ITopicRepository _topics = topics;
        private readonly IColumnRepository _columns = columns;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<ColumnId>> Handle(
            AddColumnToTopicCommand request,
            CancellationToken cancellationToken
        )
        {
            var topic = await _topics.GetTopicAsync(request.TopicId);

            if (topic is null)
            {
                return Result.Fail(Error.NotFound<ColumnId>());
            }

            var id = new ColumnId(request.TopicId, request.Requester.Id);

            var column = await _columns.GetColumnAsync(id);

            column = Column.CreateNewColumn(request.TopicId, request.Requester.Id);

            await _unitOfWork.CommitChangesAsync(cancellationToken);

            return Result.Return(column.Id);
        }
    }
}
