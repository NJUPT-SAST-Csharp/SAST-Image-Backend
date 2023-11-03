using SastImg.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Configure the config provider.
var configuration = builder.Services.ConfigureConfig(builder.Environment);

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
