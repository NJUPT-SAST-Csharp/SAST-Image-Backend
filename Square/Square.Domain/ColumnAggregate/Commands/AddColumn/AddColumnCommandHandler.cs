using FoxResult;
using Primitives.Command;
using Square.Domain.ColumnAggregate.ColumnEntity;
using Square.Domain.TopicAggregate;

namespace Square.Domain.ColumnAggregate.Commands.AddColumn
{
    internal sealed class AddColumnCommandHandler(
        ITopicRepository topics,
        IColumnRepository columns
    ) : ICommandRequestHandler<AddColumnCommand, Result<ColumnId>>
    {
        private readonly ITopicRepository _topics = topics;
        private readonly IColumnRepository _columns = columns;

        public async Task<Result<ColumnId>> Handle(
            AddColumnCommand request,
            CancellationToken cancellationToken
        )
        {
            var result = await Column.AddColumnAsync(request, _topics, _columns);

            if (result.IsFailure)
            {
                return Result.From<ColumnId>(result);
            }

            return Result.Return(result.Value);
        }
    }
}
