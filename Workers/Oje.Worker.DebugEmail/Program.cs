using Microsoft.AspNetCore.Http;
using Oje.Infrastructure;
using Oje.Security;
using Oje.Worker.DebugEmail;

IHost host = Host.CreateDefaultBuilder(args)
     .UseWindowsService(options =>
     {
         options.ServiceName = "DebugEmail";
     })
    .ConfigureServices((hostContext, services) =>
    {
        GlobalConfig.Configuration = hostContext.Configuration;
        services.AddScoped<IHttpContextAccessor, FakeIHttpContextAccessor>();
        SecurityConfig.ConfigForWorker(services);
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
