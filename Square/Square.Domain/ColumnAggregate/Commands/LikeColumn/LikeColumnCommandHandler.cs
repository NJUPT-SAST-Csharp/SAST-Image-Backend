using FoxResult;
using Primitives;
using Primitives.Command;

namespace Square.Domain.ColumnAggregate.Commands.LikeColumn
{
    internal sealed class LikeColumnCommandHandler(
        IColumnRepository repository,
        IUnitOfWork unitOfWork
    ) : ICommandRequestHandler<LikeColumnCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
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

            await _unitOfWork.CommitChangesAsync(cancellationToken);

            return Result.Success;
        }
    }
}
