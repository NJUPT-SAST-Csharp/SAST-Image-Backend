using SNS.Infrastructure.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.ConfigureMediator();
builder.Services.ConfigureEventBus(builder.Configuration);
builder.Services.ConfigureDbContext(builder.Configuration.GetConnectionString("SNSDb")!);
builder.Services.ConfigureRepositories();
builder.Services.ConfigureStorages(builder.Configuration);

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
