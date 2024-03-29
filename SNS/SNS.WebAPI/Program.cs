using SNS.Infrastructure.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ConfigureLogger();

// Add services to the container.
builder.Services.AddControllers();

builder.Services.ConfigureMediator();
builder.Services.ConfigureEventBus(builder.Configuration);
builder.Services.ConfigureDatabase(builder.Configuration.GetConnectionString("SNSDb")!);
builder.Services.ConfigureRepositories();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
