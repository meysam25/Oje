using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Interfac;
using Oje.Infrastructure.Services;
using Oje.Infrastructure.Services.ModelBinder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            ManageModalResource.Copy();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.Configure<FormOptions>(options =>
            //{
            //    options.ValueLengthLimit = 1024 * 1024 * 5; // 100MB max len form data
            //});
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(typeof(CustomExceptionFilter));
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
                options.MimeTypes = new[] { "text/plain", "application/json", "text/json" };
            });
            services.AddSignalR();
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = content =>
                {
                    bool addExteras = false;
                    if (content.File.Name.EndsWith(".js.gz"))
                    {
                        content.Context.Response.Headers["Content-Type"] = "text/javascript";
                        addExteras = true;
                    }
                    else if (content.File.Name.EndsWith(".css.gz"))
                    {
                        addExteras = true;
                        content.Context.Response.Headers["Content-Type"] = "text/css";
                    }

                    if (addExteras == true)
                    {
                        TimeSpan maxAge = new TimeSpan(1, 0, 0, 0);
                        content.Context.Response.Headers["Content-Encoding"] = "gzip";
                        content.Context.Response.Headers["Cache-Control"] = "max-age=" + maxAge.TotalSeconds.ToString("0");
                    }
                }
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

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
