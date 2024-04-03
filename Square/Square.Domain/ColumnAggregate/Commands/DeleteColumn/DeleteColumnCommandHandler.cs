using FoxResult;
using Primitives.Command;

namespace Square.Domain.ColumnAggregate.Commands.DeleteColumn
{
    internal sealed class DeleteColumnCommandHandler(IColumnRepository repository)
        : ICommandRequestHandler<DeleteColumnCommand, Result>
    {
        private readonly IColumnRepository _repository = repository;

        public async Task<Result> Handle(
            DeleteColumnCommand request,
            CancellationToken cancellationToken
        )
        {
            var column = await _repository
                .GetColumnAsync(request.TopicId, request.Requester.Id)
                .WaitAsync(cancellationToken);

            if (column is null)
            {
                return Result.Fail(Error.NotFound(column));
            }

            var result = column.DeleteColumn(request, _repository);

            return result;
        }
    }
}
