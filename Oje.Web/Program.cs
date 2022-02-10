using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Oje.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel(options => 
                    {
                        options.AddServerHeader = false;
                        //options.Listen(IPAddress.Any, 80);
                        //options.Listen(IPAddress.Any, 443, listenOptions =>
                        //{
                        //    //listenOptions.UseHttps("certificate.pfx", "1");
                        //});
                    });
                    webBuilder.UseUrls("https://localhost:5001");
                    webBuilder.UseIISIntegration();
                    webBuilder.UseStartup<Startup>();
                });
    }
}
