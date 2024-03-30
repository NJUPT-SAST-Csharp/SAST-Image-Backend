using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Primitives.Query;

namespace Square.Application.Behaviors
{
    public sealed class QueryLoggingBehavior<TRequest, TResponse>(
        ILogger<QueryLoggingBehavior<TRequest, TResponse>> logger
    ) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IQueryRequest<TResponse>
    {
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken
        )
        {
            QueryLogger.LogEnterInfo(logger, typeof(TRequest).Name);

            var response = await next().WaitAsync(cancellationToken);

            QueryLogger.LogExitInfo(logger, typeof(TRequest).Name, typeof(TResponse).Name);

            return response;
        }
    }

    public static partial class QueryLogger
    {
        [LoggerMessage(LogLevel.Information, "Handling query [{QueryName}]")]
        public static partial void LogEnterInfo(ILogger logger, string queryName);

        [LoggerMessage(LogLevel.Information, "Query [{QueryName}] handled - response: {Response}")]
        public static partial void LogExitInfo(ILogger logger, string queryName, string response);
    }
}
