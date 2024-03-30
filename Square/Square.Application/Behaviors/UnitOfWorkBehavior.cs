using MediatR;
using Microsoft.Extensions.Logging;
using Primitives;
using Primitives.Command;

namespace Square.Application.Behaviors
{
    public sealed class UnitOfWorkBehavior<TCommand, TResponse>(
        IUnitOfWork unitOfWork,
        ILogger<UnitOfWorkBehavior<TCommand, TResponse>> logger
    ) : IPipelineBehavior<TCommand, TResponse>
        where TCommand : ICommandRequest<TResponse>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<UnitOfWorkBehavior<TCommand, TResponse>> _logger = logger;

        public async Task<TResponse> Handle(
            TCommand request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken
        )
        {
            var response = await next().WaitAsync(cancellationToken);

            UnitOfWorkLogger.LogBeginTransaction(_logger);

            await _unitOfWork.CommitChangesAsync(cancellationToken);

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
}
