using System.Text.Json;
using Infrastructure;
using WebAPI.Exceptions;
using WebAPI.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddUserServices(builder.Configuration).AddJwtAuth(builder.Configuration);
builder.Services.AddAlbumServices();
builder.Services.AddImageServices();
builder.Services.AddCategoryServices();

builder.Services.AddExceptionHandlers();

builder.Logging.AddLogger();

builder.Services.AddResponseCaching();

builder
    .Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.Converters.Add(new JsonStringLongConverter());
    });

var app = builder.Build();

app.UseCors(cors =>
    cors.AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed(origin => true).AllowCredentials()
);

app.UseResponseCaching();

app.UseExceptionHandler(op => { });

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
