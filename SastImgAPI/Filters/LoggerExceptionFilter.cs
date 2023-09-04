using Microsoft.AspNetCore.Mvc.Filters;

namespace SastImgAPI.Filters
{
    public class LoggerExceptionFilter : IAsyncExceptionFilter
    {
        private readonly ILogger<LoggerExceptionFilter> _logger;

        public LoggerExceptionFilter(ILogger<LoggerExceptionFilter> logger)
        {
            _logger = logger;
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            _logger.LogError(context.Exception.Message);
            return Task.CompletedTask;
        }
    }
}
