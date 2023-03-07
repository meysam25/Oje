using Microsoft.AspNetCore.Http;
using Oje.BackupService;
using Oje.Infrastructure;
using Oje.Worker.Backup;

IHost host = Host.CreateDefaultBuilder(args)
     .UseWindowsService(options =>
     {
         options.ServiceName = "Backup";
     })
    .ConfigureServices((hostContext, services) =>
    {
        GlobalConfig.Configuration = hostContext.Configuration;
        services.AddSingleton<IHttpContextAccessor, FakeIHttpContextAccessor>();
        BackupServiceConfig.Config(services);
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
