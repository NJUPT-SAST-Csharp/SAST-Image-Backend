using SastImg.Infrastructure.Extensions;
using SastImg.WebAPI.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Configure the config provider.
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json")
    .Build();

// Configure the logger.
builder.Logging.ConfigureLogger();

builder.Services.AddExceptionHandler<BusinessRuleInvalidExceptionHandler>();

// Add & Configure services.
builder.ConfigureServices(configuration);

// Build the web application.
var app = builder.Build();

// Add & Configure services.
app.ConfigureApplication();

// Start the web application.
app.Run();
