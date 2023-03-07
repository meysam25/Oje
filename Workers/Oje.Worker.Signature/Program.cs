using Microsoft.AspNetCore.Http;
using Oje.Infrastructure;
using Oje.Security;
using Oje.ValidatedSignature;
using Oje.Worker.Signature;

IHost host = Host.CreateDefaultBuilder(args)
     .UseWindowsService(options =>
     {
         options.ServiceName = "SignatureSmsSender";
     })
    .ConfigureServices((hostContext, services) =>
    {
        GlobalConfig.Configuration = hostContext.Configuration;
        services.AddSingleton<IHttpContextAccessor, FakeIHttpContextAccessor>();
        ValidatedSignatureConfig.ConfigForWorker(services);
        SecurityConfig.ConfigForWorker(services);
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
