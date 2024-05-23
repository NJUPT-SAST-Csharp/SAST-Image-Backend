using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Square.Infrastructure.Configurations;
using Square.Infrastructure.Persistence;
using Square.WebAPI;

var builder = WebApplication.CreateBuilder(args);

builder.AddNpgsqlDbContext<SquareDbContext>(
    "SquareDb",
    settings => settings.DisableRetry = true,
    options => options.UseSnakeCaseNamingConvention()
);

// Add services to the container.

builder.Logging.ConfigureLogger();

if (builder.Environment.IsDevelopment())
{
    builder.Services.ConfigureSwagger();
}

builder
    .Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.WriteAsString;
    });

builder.ConfigureServices();

var app = builder.Build();

app.UseExceptionHandler(_ => { });

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => options.EnablePersistAuthorization());
}

await using (var scope = app.Services.CreateAsyncScope())
{
    await scope.ServiceProvider.GetRequiredService<SquareDbContext>().Database.EnsureCreatedAsync();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
