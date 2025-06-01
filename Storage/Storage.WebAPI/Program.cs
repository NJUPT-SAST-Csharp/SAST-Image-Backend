using Storage.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.AddServiceDefaults();

builder.Services.AddStorage(options =>
{
    options.Endpoint =
        configuration["AWS_ENDPOINT_URL_S3"]?.Replace("http://", string.Empty)
        //TODO: Remove http:// for localstack compatibility
        ?? throw new NullReferenceException("AWS_ENDPOINT_URL_S3 is not set in configuration.");
    options.AccessKey =
        configuration["AWS_ACCESS_KEY_ID"]
        ?? throw new NullReferenceException("AWS_ACCESS_KEY_ID is not set in configuration.");
    options.SecretKey =
        configuration["AWS_SECRET_ACCESS_KEY"]
        ?? throw new NullReferenceException("AWS_SECRET_ACCESS_KEY is not set in configuration.");
});

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapGet("/weatherforecast", () => { });

app.Run();
