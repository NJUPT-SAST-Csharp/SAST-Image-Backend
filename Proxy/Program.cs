using Proxy.Auth;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddServiceDiscovery();

builder.Services.ConfigureAuth(
    builder.Configuration["Authentication:SecKey"] ?? throw new NullReferenceException()
);

builder
    .Services.AddReverseProxy()
    .AddTransforms<AuthTransformProvider>()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddServiceDiscoveryDestinationResolver();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();

app.MapReverseProxy();

app.Run();
