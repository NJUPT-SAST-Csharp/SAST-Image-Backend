using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Compact;

namespace SastImg.Infrastructure.Extensions
{
    public static class ILoggerBuilderExtension
    {
        public static void ConfigureLogger(this ILoggingBuilder loggerBuilder)
        {
            var logger = new LoggerConfiguration().Enrich
                .FromLogContext()
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{Context}] {Message:lj}{NewLine}{Exception}"
                )
                .WriteTo.File(new CompactJsonFormatter(), "logs/logs")
                .CreateLogger();
            loggerBuilder.AddSerilog(logger);
        }
    }
}
