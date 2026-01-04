using SastImg.Infrastructure.Configurations;
using SastImg.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add & Configure services.
builder.ConfigureServices();

builder.AddServiceDefaults();
builder.AddRedisClient("Cache");
builder.AddPersistenceSupport<SastImgDbContext>();

// Build the web application.
var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    await scope
        .ServiceProvider.GetRequiredService<SastImgDbContext>()
        .Database.EnsureCreatedAsync();
}

app.MapDefaultEndpoints();

// Add & Configure services.
app.ConfigureApplication();

// Start the web application.
app.Run();
