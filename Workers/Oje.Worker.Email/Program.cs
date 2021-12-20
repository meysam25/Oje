using Microsoft.AspNetCore.Http;
using Oje.EmailService;
using Oje.Infrastructure;
using Oje.Worker.Email;

IHost host = Host.CreateDefaultBuilder(args)
     .UseWindowsService(options =>
     {
         options.ServiceName = "EmailSender";
     })
    .ConfigureServices((hostContext, services) =>
    {
        GlobalConfig.Configuration = hostContext.Configuration;
        services.AddScoped<IHttpContextAccessor, FakeIHttpContextAccessor>();
        EmailServiceConfig.Config(services);
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
