var builder = DistributedApplication.CreateBuilder(args);

var authentication = builder.AddParameter("Auth-SecKey", true);

var username = builder.AddParameter("Username", true);
var password = builder.AddParameter("Password", true);

var redis = builder.AddRedis("Cache", 6379);
var minio = builder.AddMinIO("MinIO", username, password, 9000, 9001);
var postgres = builder.AddPostgres("PostgreSQL", username, password, 5432)
//.WithDataVolume()
;

var storage = builder
    .AddProject<Projects.Storage_WebAPI>("Storage")
    .WaitFor(minio)
    .WithReference(minio);

var database = postgres.AddDatabase("SNSDb", "sastimg_sns");
var sns = builder.AddProject<Projects.SNS_WebAPI>("SNS").WaitFor(database).WithReference(database);

database = postgres.AddDatabase("SastimgDb", "sastimg");
var sastimg = builder
    .AddProject<Projects.SastImg_WebAPI>("SastImg")
    .WaitFor(database)
    .WithReference(database)
    .WithReference(redis);

database = postgres.AddDatabase("AccountDb", "sastimg_account");
var account = builder
    .AddProject<Projects.Account_WebAPI>("Account")
    .WaitFor(database)
    .WithReference(database)
    .WithReference(redis)
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

proxy
    .WithExternalHttpEndpoints()
    .WithReference(storage)
    .WithReference(sastimg)
    .WithReference(account)
    .WithReference(sns);

var app = builder.Build();

app.Run();
