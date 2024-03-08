using System.Reflection;
using Account.Infrastructure.Configurations;
using Account.WebAPI.Configurations;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ConfigureLogger();

builder
    .Configuration.AddJsonFile("appsettings.json")
    .AddJsonFile("appsettings.Development.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json");

builder.Services.ConfigureJsonSerializer();
builder.Services.ConfigureServices(builder.Configuration);
builder.Services.AddValidatorsFromAssembly(Assembly.GetCallingAssembly());
builder.Services.RegisterEndpointMappersFromAssembly(Assembly.GetCallingAssembly());

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

app.UseExceptionHandler(_ => { });

app.UseAuthentication();

app.UseAuthorization();

app.MapEndpoints();

app.Run();
