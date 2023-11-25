using Account.Application.Configurations;
using Account.Infrastructure.Configurations;
using Account.WebAPI.Endpoints;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Logging.ConfigureLogger();

builder
    .Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile("appsettings.Development.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json");

builder
    .Services
    .ConfigureHttpJsonOptions(options =>
    {
        options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
    })
    .AddPersistence(
        builder.Configuration.GetConnectionString("AccountDb") ?? throw new NullReferenceException()
    );

var app = builder.Build();

app.MapEndpoints();

app.Run();
