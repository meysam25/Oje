using Microsoft.AspNetCore.Http;
using Oje.AccountService;
using Oje.Infrastructure;
using Oje.Worker.ExternalNotificationService;

IHost host = Host.CreateDefaultBuilder(args)
     .UseWindowsService(options =>
     {
         options.ServiceName = "EmailSender";
     })
    .ConfigureServices((hostContext, services) =>
    {
        GlobalConfig.Configuration = hostContext.Configuration;
        services.AddSingleton<IHttpContextAccessor, FakeIHttpContextAccessor>();
        AccountConfig.ConfigForWorker(services);
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
