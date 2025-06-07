var builder = DistributedApplication.CreateBuilder(args);

var authentication = builder.AddParameter("Auth-SecKey", true);

var username = builder.AddParameter("Username", true);
var password = builder.AddParameter("Password", true);

var redis = builder.AddRedis("Cache", 6379, password);
var minio = builder.AddMinIO("MinIO", username, password, 9000, 9001);
var postgres = builder.AddPostgres("PostgreSQL", username, password, 5432);
var orleans = builder.AddOrleans("Orleans").WithClustering(redis).WithGrainStorage(redis);

var storage = builder
    .AddProject<Projects.Storage_WebAPI>("Storage")
    .WaitFor(minio)
    .WaitFor(redis)
    .WithReference(orleans)
    .WithReference(minio)
    .WithReference(redis);

var database = postgres.AddDatabase("SastimgDb", "sastimg");
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

//database = postgres.AddDatabase("SNSDb", "sastimg_sns");
//var sns = builder.AddProject<Projects.SNS_WebAPI>("SNS").WaitFor(database).WithReference(database);

//var proxy = builder
//    .AddProject<Projects.Proxy>("Proxy")
//    .WithEnvironment("Authentication:SecKey", authentication)
//    .WithExternalHttpEndpoints()
//    .WithReference(storage)
//    .WithReference(sastimg)
//    .WithReference(account)
//    .WithReference(sns);

var app = builder.Build();

app.Run();
