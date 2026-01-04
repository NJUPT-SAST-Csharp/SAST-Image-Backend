using Storage.Infrastructure;
using Storage.Infrastructure.Models;
using Storage.WebAPI.Endpoint;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.WebHost.UseKestrel(options =>
{
    options.Limits.MaxRequestBodySize = ImageFile.DefaultMaxSize;
});

builder.AddServiceDefaults();
builder.AddRedisClient(nameof(StackExchange.Redis));
builder.AddKeyedRedisClient(nameof(StackExchange.Redis));

builder.UseOrleans();

builder.Services.AddMinIO(options =>
{
    //TODO: Remove http:// for localstack compatibility
    options.Endpoint =
        configuration["AWS_ENDPOINT_URL_S3"]?.Replace("http://", string.Empty)
        ?? throw new NullReferenceException("AWS_ENDPOINT_URL_S3 is not set in configuration.");
    options.AccessKey =
        configuration["AWS_ACCESS_KEY_ID"]
        ?? throw new NullReferenceException("AWS_ACCESS_KEY_ID is not set in configuration.");
    options.SecretKey =
        configuration["AWS_SECRET_ACCESS_KEY"]
        ?? throw new NullReferenceException("AWS_SECRET_ACCESS_KEY is not set in configuration.");
});

builder.AddServices();

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapStorageEndpoints();

app.Run();
