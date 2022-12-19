using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace Oje.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build();
            try
            {
                CreateHostBuilder(args, config).Build().Run();
            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText("C:\\Publish\\NewCore\\wwwroot\\errText.text", ex.Message);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args, IConfigurationRoot config) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {

                    webBuilder.UseKestrel(options =>
                    {
                        options.AddServerHeader = false;

                        options.Limits.MaxRequestBodySize = 10971520;

                        options.Listen(IPAddress.Any, 80);
                        var allUrls = config.GetSection("httpsUrls").Get<List<string>>();
                        if(allUrls != null && allUrls.Count > 0)
                        {
                            options.Listen(IPAddress.Any, config.GetSection("httpsPort").Get<int>(), listenOptions =>
                            {
                                listenOptions.UseHttps(httpsOptions =>
                                {

                                    var certs = new Dictionary<string, X509Certificate2>(StringComparer.OrdinalIgnoreCase);
                                    if (allUrls != null && allUrls.Count > 0)
                                    {
                                        foreach (var url in allUrls)
                                        {
                                            var foundCert = CertificateLoader.LoadFromStoreCert(url, "My", StoreLocation.LocalMachine, true);
                                            certs.Add(url, foundCert);
                                        }
                                    }

                                    httpsOptions.ServerCertificateSelector = (connectionContext, name) =>
                                    {
                                        if (name is not null && certs.TryGetValue(name, out var cert))
                                        {
                                            return cert;
                                        }

                                        return null;
                                    };
                                });
                            });
                        }
                      
                    });
                    webBuilder.UseUrls();
                    //webBuilder.UseIISIntegration();
                    webBuilder.UseStartup<Startup>();
                })
            .UseWindowsService()
            ;
    }
}
