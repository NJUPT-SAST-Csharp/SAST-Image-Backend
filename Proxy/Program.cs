var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddServiceDiscovery();

builder
    .Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddServiceDiscoveryDestinationResolver();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapReverseProxy();
app.Run();
