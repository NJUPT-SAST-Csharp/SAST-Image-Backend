using System.Text.Json.Serialization;
using Square.Infrastructure.Configurations;
using Square.WebAPI;

var builder = WebApplication.CreateBuilder(args);

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

app.UseAuthorization();

app.MapControllers();

app.Run();
