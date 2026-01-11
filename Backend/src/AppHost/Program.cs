var builder = DistributedApplication.CreateBuilder(args);

var authentication = builder.AddParameter("Auth-SecKey", true);
var username = builder.AddParameter("Username", true);
var password = builder.AddParameter("Password", true);

var garnet = builder.AddGarnet("cache", 6379, password);
var postgres = builder.AddPostgres("db", username, password, 5432);

var orleansClustering = garnet;
var orleansStorage = garnet;
var orleans = builder
    .AddOrleans("Orleans")
    .WithClustering(orleansClustering)
    .WithGrainStorage(orleansStorage);

var app = builder.Build();

app.Run();
