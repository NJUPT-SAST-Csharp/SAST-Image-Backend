using MediatR;
using Microsoft.Extensions.Logging;
using Primitives.Command;

namespace Square.Application.Behaviors
{
    public sealed class CommandLoggingBehavior<TRequest, TResponse>(
        ILogger<CommandLoggingBehavior<TRequest, TResponse>> logger
    ) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommandRequest<TResponse>
    {
        private readonly ILogger<CommandLoggingBehavior<TRequest, TResponse>> _logger = logger;

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken
        )
        {
            CommandLogger.LogEnterInfo(_logger, typeof(TRequest).Name);

            var response = await next().WaitAsync(cancellationToken);

            CommandLogger.LogExitInfo(
                _logger,
                typeof(TRequest).Name,
                response?.ToString() ?? string.Empty
            );

            return response;
        }
    }

    public static partial class CommandLogger
    {
        [LoggerMessage(LogLevel.Information, "Handling command [{CommandName}]")]
        public static partial void LogEnterInfo(ILogger logger, string commandName);

        [LoggerMessage(
            LogLevel.Information,
            "Command [{CommandName}] handled - response: {Response}"
        )]
        public static partial void LogExitInfo(ILogger logger, string commandName, string response);
    }
}
