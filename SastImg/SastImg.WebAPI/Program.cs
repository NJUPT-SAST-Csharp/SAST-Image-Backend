using Microsoft.EntityFrameworkCore;
using SastImg.Infrastructure.Configurations;
using SastImg.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.AddRedisClient("Cache");
builder.AddNpgsqlDbContext<SastImgDbContext>(
    "SastimgDb",
    settings => settings.DisableRetry = true,
    options => options.UseSnakeCaseNamingConvention()
);

// Configure the config provider.
builder
    .Configuration.AddJsonFile("appsettings.json")
    .AddJsonFile("appsettings.Development.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json")
    .Build();

// Configure the logger.
builder.Logging.ConfigureLogger();

// Add & Configure services.
builder.ConfigureServices();

// Build the web application.
var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    await scope
        .ServiceProvider.GetRequiredService<SastImgDbContext>()
        .Database.EnsureCreatedAsync();
}

// Add & Configure services.
app.ConfigureApplication();

// Start the web application.
app.Run();
