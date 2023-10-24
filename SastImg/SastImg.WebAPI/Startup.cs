using Serilog;
using Serilog.Formatting.Compact;
using ILogger = Serilog.ILogger;

namespace SastImg.WebAPI
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env) { }

        private static ILogger ConfigureLogger()
        {
            return new LoggerConfiguration().Enrich
                .FromLogContext()
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{Context}] {Message:lj}{NewLine}{Exception}"
                )
                .WriteTo.File(new CompactJsonFormatter(), "logs/logs")
                .CreateLogger();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) { }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            return services.BuildServiceProvider();
        }
    }
}
