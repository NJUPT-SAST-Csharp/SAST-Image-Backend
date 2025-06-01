using System.Reflection;
using Account.Infrastructure.Configurations;
using Account.Infrastructure.Persistence;
using Account.WebAPI.Configurations;
using Auth;
using ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureServices();

builder.AddServiceDefaults();
builder.AddRedisClient("Cache");
builder.EnrichPersistence<AccountDbContext>();

//builder
//    .Configuration.AddJsonFile("appsettings.json")
//    .AddJsonFile("appsettings.Development.json")
//    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json");

builder.Services.ConfigureJsonSerializer();
builder.Services.RegisterEndpointMappersFromAssembly(Assembly.GetAssembly(typeof(Program))!);

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

app.MapDefaultEndpoints();

app.MapEndpoints();

app.Run();
