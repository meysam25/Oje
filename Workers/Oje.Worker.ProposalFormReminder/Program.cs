using Microsoft.AspNetCore.Http;
using Oje.Infrastructure;
using Oje.ProposalFormWorker;
using Oje.Worker.ProposalFormReminder;

IHost host = Host.CreateDefaultBuilder(args)
     .UseWindowsService(options =>
     {
         options.ServiceName = "ProposalFormReminder";
     })
    .ConfigureServices((hostContext, services) =>
    {
        GlobalConfig.Configuration = hostContext.Configuration;
        services.AddSingleton<IHttpContextAccessor, FakeIHttpContextAccessor>();
        ProposalFormConfig.ConfigForWorker(services);
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
