using Microsoft.AspNetCore.Http;
using Oje.Infrastructure;
using Oje.PaymentService;
using Oje.Worker.Payment;

IHost host = Host.CreateDefaultBuilder(args)
     .UseWindowsService(options =>
     {
         options.ServiceName = "Payment";
     })
    .ConfigureServices((hostContext, services) =>
    {
        GlobalConfig.Configuration = hostContext.Configuration;
        services.AddSingleton<IHttpContextAccessor, FakeIHttpContextAccessor>();
        PaymentServiceConfig.ConfigWorker(services);
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
