using FoxResult;
using Primitives.Command;

namespace Square.Domain.ColumnAggregate.Commands.UnlikeColumn
{
    internal sealed class UnlikeColumnCommandHandler(IColumnRepository repository)
        : ICommandRequestHandler<UnlikeColumnCommand, Result>
    {
        private readonly IColumnRepository _repository = repository;

        public async Task<Result> Handle(
            UnlikeColumnCommand request,
            CancellationToken cancellationToken
        )
        {
            var column = await _repository
                .GetColumnAsync(request.ColumnId)
                .WaitAsync(cancellationToken);

            if (column is null)
            {
                return Result.Fail(Error.NotFound());
            }

            return Result.Success;
        }
    }
}
