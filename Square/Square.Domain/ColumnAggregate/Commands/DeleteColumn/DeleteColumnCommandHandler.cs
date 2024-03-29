using FoxResult;
using Primitives;
using Primitives.Command;

namespace Square.Domain.ColumnAggregate.Commands.DeleteColumn
{
    internal sealed class DeleteColumnCommandHandler(
        IUnitOfWork unitOfWork,
        IColumnRepository repository
    ) : ICommandRequestHandler<DeleteColumnCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IColumnRepository _repository = repository;

        public async Task<Result> Handle(
            DeleteColumnCommand request,
            CancellationToken cancellationToken
        )
        {
            var column = await _repository.GetColumnAsync(request.ColumnId);
            if (column is null)
            {
                return Result.Fail(Error.NotFound(column));
            }

            await _repository.DeleteColumnAsync(column).WaitAsync(cancellationToken);

            await _unitOfWork.CommitChangesAsync(cancellationToken);

            return Result.Success;
        }
    }
}
