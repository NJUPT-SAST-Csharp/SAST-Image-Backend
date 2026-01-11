using Domain.Extensions;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Infrastructure;

public static class ILoggerBuilderConfiguration
{
    public static ILoggingBuilder AddLogger(this ILoggingBuilder builder)
    {
        var logger = new LoggerConfiguration()
            .MinimumLevel.Override(
                "Microsoft.EntityFrameworkCore.Database.Command",
                Serilog.Events.LogEventLevel.Warning
            )
            .MinimumLevel.Override(
                "Microsoft.AspNetCore.Authentication",
                Serilog.Events.LogEventLevel.Warning
            )
            .Filter.ByExcluding(e => e.Exception is { } and DomainException)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();
        builder.ClearProviders().AddSerilog(logger);

        return builder;
    }
}
