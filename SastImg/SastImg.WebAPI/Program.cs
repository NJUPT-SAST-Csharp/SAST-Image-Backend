using SastImg.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Configure the config provider.
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json")
    .Build();

// Configure the logger.
builder.Logging.ConfigureLogger();

// Add & Configure services.
builder.ConfigureServices(configuration);

// Build the web application.
var app = builder.Build();

// Add & Configure services.
app.ConfigureApplication();

// Start the web application.
app.Run();
