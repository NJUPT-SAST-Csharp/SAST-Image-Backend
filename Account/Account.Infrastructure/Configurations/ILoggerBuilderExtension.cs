using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Account.Infrastructure.Configurations
{
    public static class ILoggerBuilderExtension
    {
        public static void ConfigureLogger(this ILoggingBuilder builder)
        {
            var logger = new LoggerConfiguration()
                .Enrich
                .FromLogContext()
                .WriteTo
                .Console(theme: AnsiConsoleTheme.Code)
                .CreateLogger();
            builder.ClearProviders().AddSerilog(logger);
        }
    }
}
