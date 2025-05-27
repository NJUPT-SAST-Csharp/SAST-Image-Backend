using System.Reflection;
using Account.Infrastructure.Configurations;
using Account.Infrastructure.Persistence;
using Account.WebAPI.Configurations;
using Auth;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddRedisClient("Cache");
builder.AddNpgsqlDbContext<AccountDbContext>(
    "AccountDb",
    settings => settings.DisableRetry = true,
    options => options.UseSnakeCaseNamingConvention()
);

//builder
//    .Configuration.AddJsonFile("appsettings.json")
//    .AddJsonFile("appsettings.Development.json")
//    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json");

builder.Services.ConfigureJsonSerializer();
builder.Services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(Program)));
builder.Services.RegisterEndpointMappersFromAssembly(Assembly.GetAssembly(typeof(Program))!);

builder.ConfigureServices();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    await scope
        .ServiceProvider.GetRequiredService<AccountDbContext>()
        .Database.EnsureCreatedAsync();
}

app.UseExceptionHandler(_ => { });

app.UseRouting();

app.UseInternalAuth();

app.MapEndpoints();

app.Run();
