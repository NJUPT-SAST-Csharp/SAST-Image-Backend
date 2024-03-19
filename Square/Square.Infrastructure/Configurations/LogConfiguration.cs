using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Compact;

namespace Square.Infrastructure.Configurations
{
    public static class LogConfiguration
    {
        public static void ConfigureLogger(this ILoggingBuilder loggerBuilder)
        {
            loggerBuilder.ClearProviders();
            var logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(new CompactJsonFormatter(), "logs/logs")
                .CreateLogger();

            loggerBuilder.AddSerilog(logger);
        }
    }
}
