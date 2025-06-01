using Auth;
using Microsoft.EntityFrameworkCore;
using SNS.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<SNSDbContext>(
    "SNSDb",
    settings => settings.DisableRetry = true,
    configureDbContextOptions: options => options.UseSnakeCaseNamingConvention()
);

builder.Services.AddControllers();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    await scope.ServiceProvider.GetRequiredService<SNSDbContext>().Database.EnsureCreatedAsync();
}

app.MapDefaultEndpoints();

app.UseInternalAuth();

app.MapControllers();

app.Run();
