using MediatR.Pipeline;
using Primitives.Command;

namespace Primitives.Extensions.Behaviors
{
    internal sealed class UnitOfWorkPostProcessor<TCommand, TResponse>(IUnitOfWork unitOfWork)
        : IRequestPostProcessor<TCommand, TResponse>
        where TCommand : IBaseCommand
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public Task Process(
            TCommand request,
            TResponse response,
            CancellationToken cancellationToken
        )
        {
            return _unitOfWork.CommitChangesAsync(cancellationToken);
        }
    }
}
