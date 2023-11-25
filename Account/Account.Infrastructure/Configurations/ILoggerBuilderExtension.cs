using Microsoft.Extensions.Logging;
using Serilog;

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
                .Console()
                .CreateLogger();
            builder.AddSerilog(logger);
        }
    }
}
