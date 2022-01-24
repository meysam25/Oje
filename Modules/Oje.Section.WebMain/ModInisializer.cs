using Oje.Infrastructure.Interfac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Oje.Section.WebMain.Interfaces;
using Oje.Section.WebMain.Services;
using Oje.Section.WebMain.Services.EContext;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure;

namespace Oje.Section.WebMain
{
    public class ModInisializer : IModInisializer
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<WebMainDBContext>(options => options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"], b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)));

            services.AddScoped<ITopMenuService, TopMenuService>();
        }
    }
}
