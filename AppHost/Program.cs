var builder = DistributedApplication.CreateBuilder(args);

var storage = builder.AddParameter("StoragePath");

var rabbitmq = builder.AddRabbitMQ(
    "RabbitMQ",
    builder.AddParameter("MQUsername"),
    builder.AddParameter("MQPassword"),
    5672
);

var redis = builder.AddRedis("Cache");

var postgres = builder
    .AddPostgres(
        "PostgreSQL",
        builder.AddParameter("PsqlUsername"),
        builder.AddParameter("PsqlPassword"),
        5432
    )
    .WithDataVolume();

var square = builder
    .AddProject<Projects.Square_WebAPI>("Square")
    .WithReference(postgres.AddDatabase("SquareDb", "sastimg_square"))
    .WithEnvironment("StoragePath", storage);

var sns = builder
    .AddProject<Projects.SNS_WebAPI>("SNS")
    .WithReference(postgres.AddDatabase("SNSDb", "sastimg_sns"))
    .WithReference(rabbitmq)
    .WithEnvironment("StoragePath", storage);

var sastimg = builder
    .AddProject<Projects.SastImg_WebAPI>("SastImg")
    .WithReference(postgres.AddDatabase("SastimgDb", "sastimg"))
    .WithReference(redis)
    .WithReference(rabbitmq)
    .WithEnvironment("StoragePath", storage);

var account = builder
    .AddProject<Projects.Account_WebAPI>("Account")
    .WithReference(postgres.AddDatabase("AccountDb", "sastimg_account"))
    .WithReference(redis)
    .WithReference(rabbitmq)
    .WithEnvironment("StoragePath", storage);

var proxy = builder.AddProject<Projects.Proxy>("Proxy");

builder
    .AddNpmApp("Frontend", builder.Configuration["Parameters:FrontendDirectory"]!, "dev")
    .WithReference(proxy)
    .WithHttpEndpoint(5173, env: "PORT", isProxied: false)
    .WithExternalHttpEndpoints();

proxy
    .WithExternalHttpEndpoints()
    .WithReference(sastimg)
    .WithReference(account)
    .WithReference(sns)
    .WithReference(square);

builder.Build().Run();
