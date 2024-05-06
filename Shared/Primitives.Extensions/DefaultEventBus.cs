using MediatR;
using Microsoft.Extensions.Logging;
using Primitives.Command;
using Primitives.DomainEvent;
using Primitives.Query;
using Shared.Primitives.DomainEvent;
using Shared.Primitives.Query;

namespace Primitives
{
    internal sealed class DefaultEventBus(
        IMediator mediator,
        ILogger<ICommandRequestSender> commandLogger,
        ILogger<IQueryRequestSender> queryLogger
    ) : IQueryRequestSender, ICommandRequestSender, IDomainEventPublisher
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<ICommandRequestSender> _commandLogger = commandLogger;
        private readonly ILogger<IQueryRequestSender> _queryLogger = queryLogger;

        public async Task<TResponse> CommandAsync<TResponse>(
            ICommandRequest<TResponse> command,
            CancellationToken cancellationToken = default
        )
        {
            int commandId = Random.Shared.Next();
            string commandName = command.ToString() ?? command.GetType().Name;

            CommandLogger.LogEnterInfo(_commandLogger, commandId, commandName);

            var result = await _mediator.Send(command, cancellationToken);

            string resultName = result?.ToString() ?? result?.GetType().Name ?? "Null";

            CommandLogger.LogExitInfo(_commandLogger, commandId, commandName, resultName);

            return result;
        }

        public async Task CommandAsync(
            ICommandRequest command,
            CancellationToken cancellationToken = default
        )
        {
            int commandId = Random.Shared.Next();
            string commandName = command.ToString() ?? command.GetType().Name;

            CommandLogger.LogEnterInfo(_commandLogger, commandId, commandName);

            await _mediator.Send(command, cancellationToken);

            CommandLogger.LogExitInfo(_commandLogger, commandId, commandName);
        }

        public Task PublishAsync<TEvent>(
            TEvent domainEvent,
            CancellationToken cancellationToken = default
        )
            where TEvent : IDomainEvent
        {
            return _mediator.Publish(domainEvent, cancellationToken);
        }

        public async Task<TResponse> QueryAsync<TResponse>(
            IQueryRequest<TResponse> request,
            CancellationToken cancellationToken = default
        )
        {
            int queryId = Random.Shared.Next();
            string requestName = request.ToString() ?? request.GetType().Name;

            QueryLogger.LogEnterInfo(_queryLogger, queryId, requestName);

            var result = await _mediator.Send(request, cancellationToken);

            string resultName = result?.ToString() ?? result?.GetType().Name ?? "Null";

            QueryLogger.LogExitInfo(_queryLogger, queryId, requestName, resultName);

            return result;
        }
    }

    internal static partial class CommandLogger
    {
        [LoggerMessage(LogLevel.Information, "[{Id}] Command [{CommandName}] handling...")]
        public static partial void LogEnterInfo(ILogger logger, int id, string commandName);

        [LoggerMessage(
            LogLevel.Information,
            "[{Id}] Command [{CommandName}] handled - response: {Response}"
        )]
        public static partial void LogExitInfo(
            ILogger logger,
            int id,
            string commandName,
            string response
        );

        [LoggerMessage(
            LogLevel.Information,
            "[{Id}] Command [{CommandName}] handled - No response."
        )]
        public static partial void LogExitInfo(ILogger logger, int id, string commandName);
    }

    internal static partial class QueryLogger
    {
        [LoggerMessage(LogLevel.Information, "[{Id}] Query [{QueryName}] handling...")]
        public static partial void LogEnterInfo(ILogger logger, int id, string queryName);

        [LoggerMessage(
            LogLevel.Information,
            "[{Id}] Query [{QueryName}] handled - response: {Response}"
        )]
        public static partial void LogExitInfo(
            ILogger logger,
            int id,
            string queryName,
            string response
        );
    }
}
