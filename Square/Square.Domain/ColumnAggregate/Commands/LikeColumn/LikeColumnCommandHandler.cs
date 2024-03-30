using FoxResult;
using Primitives.Command;

namespace Square.Domain.ColumnAggregate.Commands.LikeColumn
{
    internal sealed class LikeColumnCommandHandler(IColumnRepository repository)
        : ICommandRequestHandler<LikeColumnCommand, Result>
    {
        private readonly IColumnRepository _repository = repository;

        public async Task<Result> Handle(
            LikeColumnCommand request,
            CancellationToken cancellationToken
        )
        {
            var column = await _repository.GetColumnAsync(request.ColumnId);

            if (column is null)
            {
                return Result.Fail(Error.NotFound());
            }

            return Result.Success;
        }
    }
}
