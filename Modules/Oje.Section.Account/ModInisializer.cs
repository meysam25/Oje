using Oje.AccountService;
using Oje.Infrastructure.Interfac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Oje.AccountService.Hubs;
using Oje.AccountService.Interfaces;

namespace Oje.Section.Account
{
    public class ModInisializer : IModInisializer
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<NotificationHub>("/notification");
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            AccountConfig.Config(services);

            var sBuilder = services.BuildServiceProvider();
            sBuilder.GetService<ISectionService>().UpdateModuals();
            sBuilder.GetService<ITempSqlCommService>().SetFlagForGooglePointPerformanceProblem();
        }
    }
}
