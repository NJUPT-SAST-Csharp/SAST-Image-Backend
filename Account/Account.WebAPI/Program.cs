using Account.Infrastructure.Configurations;
using Account.Infrastructure.Persistence;
using Account.WebAPI;
using ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureServices();

if (builder.Environment.IsDevelopment())
{
    builder.AddServiceDefaults();
    builder.AddRedisSupport();
    builder.AddPersistenceSupport<AccountDbContext>();
}

builder.UseOrleans(options => options.AddRedisGrainStorageAsDefault());

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    await scope
        .ServiceProvider.GetRequiredService<AccountDbContext>()
        .Database.EnsureCreatedAsync();
}

app.UseExceptionHandler(_ => { });

app.UseRouting();

app.MapDefaultEndpoints();

app.MapEndpoints();

app.Run();
