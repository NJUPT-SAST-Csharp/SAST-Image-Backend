using Account.Infrastructure.Configurations;
using Account.WebAPI.Configurations;
using Account.WebAPI.Endpoints;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Logging.ConfigureLogger();

builder
    .Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile("appsettings.Development.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json");

builder.Services.ConfigureJsonSerializer();

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

app.MapEndpoints();

app.Run();
