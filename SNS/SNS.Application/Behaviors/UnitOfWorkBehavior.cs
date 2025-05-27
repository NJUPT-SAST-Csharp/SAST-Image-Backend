using Mediator;
using Microsoft.Extensions.Logging;
using Primitives;

namespace SNS.Application.Behaviors;

public sealed class UnitOfWorkBehavior<TCommand, TResponse>(
    IUnitOfWork unitOfWork,
    ILogger<UnitOfWorkBehavior<TCommand, TResponse>> logger
) : IPipelineBehavior<TCommand, TResponse>
    where TCommand : notnull, IBaseCommand
{
    private readonly ILogger<UnitOfWorkBehavior<TCommand, TResponse>> _logger = logger;

    public async ValueTask<TResponse> Handle(
        TCommand message,
        CancellationToken cancellationToken,
        MessageHandlerDelegate<TCommand, TResponse> next
    )
    {
        var response = await next(message, cancellationToken);

        UnitOfWorkLogger.LogBeginTransaction(_logger);

        await unitOfWork.CommitChangesAsync(cancellationToken);

        UnitOfWorkLogger.LogEndTransaction(_logger);

        return response;
    }
}

public static partial class UnitOfWorkLogger
{
    [LoggerMessage(LogLevel.Information, "Begin transaction.")]
    public static partial void LogBeginTransaction(ILogger logger);

    [LoggerMessage(LogLevel.Information, "End transaction.")]
    public static partial void LogEndTransaction(ILogger logger);
}
