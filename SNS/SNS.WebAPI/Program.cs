using Microsoft.EntityFrameworkCore;
using Primitives;
using SNS.Infrastructure.Configurations;
using SNS.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.AddNpgsqlDbContext<SNSDbContext>(
    "SNSDb",
    settings => settings.DisableRetry = true,
    configureDbContextOptions: options => options.UseSnakeCaseNamingConvention()
);

builder.Logging.ConfigureLogger();

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddPrimitives(options =>
{
    options.AddUnitOfWorkWithDbContext<SNSDbContext>();
    options.AddResolversFromAssemblies(
        SNS.Application.AssemblyReference.Assembly,
        SNS.Domain.AssemblyReference.Assembly
    );
    options.AutoCommitAfterCommandHandled = true;
});
builder.Services.ConfigureEventBus(builder.Configuration);
builder.Services.ConfigurePersistence(builder.Configuration);
builder.Services.ConfigureAuth(builder.Configuration);

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

await using (var scope = app.Services.CreateAsyncScope())
{
    await scope.ServiceProvider.GetRequiredService<SNSDbContext>().Database.EnsureCreatedAsync();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
