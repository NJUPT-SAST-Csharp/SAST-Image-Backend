var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject("Proxy", "../Proxy/Proxy.csproj");
builder.AddProject("Square", "../Square/Square.WebAPI/Square.WebAPI.csproj");
builder.AddProject("SastImg", "../SastImg/SastImg.WebAPI/SastImg.WebAPI.csproj");
builder.AddProject("SNS", "../SNS/SNS.WebAPI/SNS.WebAPI.csproj");
builder.AddProject("Account", "../Account/Account.WebAPI/Account.WebAPI.csproj");

builder.Build().Run();