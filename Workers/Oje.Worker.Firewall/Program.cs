using Microsoft.AspNetCore.Http;
using Oje.Infrastructure;
using Oje.Security;
using Oje.Worker.Firewall;

IHost host = Host.CreateDefaultBuilder(args)
     .UseWindowsService(options =>
     {
         options.ServiceName = "OjeFirewall";
     })
    .ConfigureServices((hostContext, services) =>
    {
        GlobalConfig.Configuration = hostContext.Configuration;
        services.AddSingleton<IHttpContextAccessor, FakeIHttpContextAccessor>();
        SecurityConfig.ConfigForWorker(services);
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
