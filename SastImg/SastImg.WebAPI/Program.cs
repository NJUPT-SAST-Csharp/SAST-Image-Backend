using SastImg.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

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

// Add & Configure services.
app.ConfigureApplication();

// Start the web application.
app.Run();
