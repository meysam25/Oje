using Oje.AccountManager;
using Oje.Infrastructure;
using Oje.Infrastructure.Interfac;
using Oje.Section.CarBodyBaseData.Interfaces;
using Oje.Section.CarBodyBaseData.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oje.Section.CarBodyBaseData.Services.EContext;

namespace Oje.Section.CarBodyBaseData
{
    public class ModInisializer : IModInisializer
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<CarBodyDBContext>(options => options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"], b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)));

            services.AddScoped<ICarSpecificationManager, CarSpecificationManager>();
            services.AddScoped<ICarSpecificationAmountManager, CarSpecificationAmountManager>();
            services.AddScoped<ICompanyManager, CompanyManager>();
            services.AddScoped<ICarBodyCreateDatePercentManager, CarBodyCreateDatePercentManager>();
        }
    }
}
