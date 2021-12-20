using Microsoft.AspNetCore.Http;
using Oje.Infrastructure;
using Oje.Sms;
using Oje.Worker.Sms;

IHost host = Host.CreateDefaultBuilder(args)
     .UseWindowsService(options =>
     {
         options.ServiceName = "SmsSender";
     })
    .ConfigureServices((hostContext, services) =>
    {
        GlobalConfig.Configuration = hostContext.Configuration;
        services.AddScoped<IHttpContextAccessor, FakeIHttpContextAccessor>();
        SmsConfig.Config(services);
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
