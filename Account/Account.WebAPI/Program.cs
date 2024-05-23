using System.Data;
using System.Reflection;
using Account.Infrastructure.Configurations;
using Account.Infrastructure.Persistence;
using Account.WebAPI.Configurations;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddRedisClient("Cache");
builder.AddNpgsqlDbContext<AccountDbContext>(
    "AccountDb",
    settings => settings.DisableRetry = true,
    options => options.UseSnakeCaseNamingConvention()
);

builder.Logging.ConfigureLogger();

builder
    .Configuration.AddJsonFile("appsettings.json")
    .AddJsonFile("appsettings.Development.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json");

builder.Services.ConfigureJsonSerializer();
builder.Services.ConfigureServices(builder.Configuration);
builder.Services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(Program)));
builder.Services.RegisterEndpointMappersFromAssembly(Assembly.GetAssembly(typeof(Program))!);

if (builder.Environment.IsDevelopment())
{
    builder.Services.ConfigureSwagger();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await using (var scope = app.Services.CreateAsyncScope())
{
    await scope
        .ServiceProvider.GetRequiredService<AccountDbContext>()
        .Database.EnsureCreatedAsync();
}

app.UseExceptionHandler(_ => { });

app.UseAuthentication();

app.UseAuthorization();

app.MapEndpoints();

app.Run();
