using Microsoft.AspNetCore;
using SastImg.WebAPI;

var builder = WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();

var host = builder.Build();

host.Run();
