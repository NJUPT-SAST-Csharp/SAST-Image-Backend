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
            CommandLogger.LogEnterInfo(_commandLogger, command.GetType().Name);

            var result = await _mediator.Send(command, cancellationToken);

            CommandLogger.LogExitInfo(
                _commandLogger,
                command.GetType().Name,
                typeof(TResponse).Name
            );

            return result;
        }

        public async Task CommandAsync(
            ICommandRequest command,
            CancellationToken cancellationToken = default
        )
        {
            CommandLogger.LogEnterInfo(_commandLogger, command.GetType().Name);

            await _mediator.Send(command, cancellationToken);

            CommandLogger.LogExitInfo(_commandLogger, command.GetType().Name);
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
            QueryLogger.LogEnterInfo(_queryLogger, request.GetType().Name);

            var result = await _mediator.Send(request, cancellationToken);

            QueryLogger.LogExitInfo(_queryLogger, request.GetType().Name, typeof(TResponse).Name);

            return result;
        }
    }

    internal static partial class CommandLogger
    {
        [LoggerMessage(LogLevel.Information, "Handling command [{CommandName}]")]
        public static partial void LogEnterInfo(ILogger logger, string commandName);

        [LoggerMessage(
            LogLevel.Information,
            "Command [{CommandName}] handled - response: {Response}"
        )]
        public static partial void LogExitInfo(ILogger logger, string commandName, string response);

        [LoggerMessage(LogLevel.Information, "Command [{CommandName}] handled - No response.")]
        public static partial void LogExitInfo(ILogger logger, string commandName);
    }

    internal static partial class QueryLogger
    {
        [LoggerMessage(LogLevel.Information, "Handling query [{QueryName}]")]
        public static partial void LogEnterInfo(ILogger logger, string queryName);

        [LoggerMessage(LogLevel.Information, "Query [{QueryName}] handled - response: {Response}")]
        public static partial void LogExitInfo(ILogger logger, string queryName, string response);
    }
}
