using SastImg.Infrastructure.Configurations;
using SastImg.Infrastructure.Persistence;
using ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

// Add & Configure services.
builder.ConfigureServices();

builder.AddServiceDefaults();
builder.AddRedisClient("Cache");
builder.EnrichPersistence<SastImgDbContext>();

// Build the web application.
var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    await scope
        .ServiceProvider.GetRequiredService<SastImgDbContext>()
        .Database.EnsureCreatedAsync();
}

// Add & Configure services.
app.ConfigureApplication();

// Start the web application.
app.Run();
