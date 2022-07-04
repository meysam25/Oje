using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Interfac;
using Oje.Infrastructure.Services.ModelBinder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Oje.AccountService.Middlewares;
using Oje.Section.Core.Services;
using Oje.Security.Filters;
using Microsoft.AspNetCore.Http.Features;

namespace Oje.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            GlobalConfig.WebHostEnvironment = hostingEnvironment;
            GlobalConfig.Configuration = configuration;
            //ManageModalResource.Copy();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin, UnicodeRanges.Arabic }));

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);

            // Add Response compression services
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(typeof(CustomExceptionFilter));
                options.Filters.Add(typeof(PreventPostRequestIfNoReferrerUrlFilter));
                options.ModelBinderProviders.Insert(0, new StringModelBinderProvider());
                options.ModelBinderProviders.Insert(0, new MyHtmlStringBinderProvider());
            }).AddJsonOptions(option =>
            {
                option.JsonSerializerOptions.Converters.Add(new JsonMyHtmlConverter());
                option.JsonSerializerOptions.Converters.Add(new JsonTextConverter());
            });
            services.AddHttpContextAccessor();
            services.AddResponseCompression(options =>
            {
                options.MimeTypes = new string[]{
                    "text/html",
                    "application/json"
                };
                options.EnableForHttps = true;
                options.Providers.Add<GzipCompressionProvider>();
            });
            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
            });
            List<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.Contains("Oje") && !a.Location.EndsWith(".Views.dll")).ToList();
            GlobalConfig.Moduals = assemblies;
            foreach (var module in assemblies)
            {
                var moduleInitializerType = module.GetTypes()
                   .FirstOrDefault(t => typeof(IModInisializer).IsAssignableFrom(t));
                if ((moduleInitializerType != null) && (moduleInitializerType != typeof(IModInisializer)))
                {
                    var moduleInitializer = (IModInisializer)Activator.CreateInstance(moduleInitializerType);
                    services.AddSingleton(typeof(IModInisializer), moduleInitializer);
                    moduleInitializer.ConfigureServices(services);
                }
            }

            services.Configure<FormOptions>(x =>
            {
                x.MultipartBodyLengthLimit = 10971520;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.HttpsStuff();
            app.UseResponseCompression();
            app.UseStaticFiles(CoreConfig.GetStaticFileOptions());

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            var moduleInitializers = app.ApplicationServices.GetServices<IModInisializer>();
            foreach (var moduleInitializer in moduleInitializers)
            {
                moduleInitializer.Configure(app, env);
            }

        }
    }
}
