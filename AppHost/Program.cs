var builder = DistributedApplication.CreateBuilder(args);

var storage = builder.AddParameter("StoragePath");
var authentication = builder.AddParameter("Auth-SecKey", true);

var rabbitmq = builder.AddRabbitMQ(
    "RabbitMQ",
    builder.AddParameter("RabbitMQ-Username", true),
    builder.AddParameter("RabbitMQ-Password", true),
    5672
);

var redis = builder.AddRedis("Cache", 6379);

var postgres = builder.AddPostgres(
    "PostgreSQL",
    builder.AddParameter("Postgres-Username", true),
    builder.AddParameter("Postgres-Password", true),
    5432
)
//.WithDataVolume()
;

var database = postgres.AddDatabase("SNSDb", "sastimg_sns");
var sns = builder
    .AddProject<Projects.SNS_WebAPI>("SNS")
    .WaitFor(database)
    .WithReference(database)
    .WithReference(rabbitmq)
    .WithEnvironment("StoragePath", storage);

database = postgres.AddDatabase("SastimgDb", "sastimg");
var sastimg = builder
    .AddProject<Projects.SastImg_WebAPI>("SastImg")
    .WaitFor(database)
    .WithReference(database)
    .WithReference(redis)
    .WithReference(rabbitmq)
    .WithEnvironment("StoragePath", storage);

database = postgres.AddDatabase("AccountDb", "sastimg_account");
var account = builder
    .AddProject<Projects.Account_WebAPI>("Account")
    .WaitFor(database)
    .WithReference(database)
    .WithReference(redis)
    .WithReference(rabbitmq)
    .WithEnvironment("StoragePath", storage)
    .WithEnvironment("Authentication:SecKey", authentication);

var proxy = builder
    .AddProject<Projects.Proxy>("Proxy")
    .WithEnvironment("Authentication:SecKey", authentication);

builder
    .AddNpmApp("Frontend", builder.Configuration["Parameters:FrontendDirectory"]!, "dev")
    .WaitFor(proxy)
    .WithReference(proxy)
    .WithHttpEndpoint(5173, env: "PORT", isProxied: false)
    .WithExternalHttpEndpoints();

proxy.WithExternalHttpEndpoints().WithReference(sastimg).WithReference(account).WithReference(sns);

var app = builder.Build();

app.Run();
