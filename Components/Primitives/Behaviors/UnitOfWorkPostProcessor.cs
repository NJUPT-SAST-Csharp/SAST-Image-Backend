using Mediator;

namespace Primitives.Behaviors;

internal sealed class UnitOfWorkPostProcessor<TCommand, TResponse>(IUnitOfWork unitOfWork)
    : MessagePostProcessor<TCommand, TResponse>
    where TCommand : IBaseCommand
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    protected override async ValueTask Handle(
        TCommand message,
        TResponse response,
        CancellationToken cancellationToken
    )
    {
        await _unitOfWork.CommitChangesAsync(cancellationToken);
    }
}
