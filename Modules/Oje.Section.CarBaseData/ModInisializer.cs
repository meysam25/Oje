using Oje.Infrastructure;
using Oje.Infrastructure.Interfac;
using Oje.Section.CarBaseData.Interfaces;
using Oje.Section.CarBaseData.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oje.Section.CarBaseData.Services.EContext;

namespace Oje.Section.CarBaseData
{
    public class ModInisializer : IModInisializer
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<CarDBContext>(options => options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"], b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)));

            services.AddScoped<ICarTypeManager, CarTypeManager>();
            services.AddScoped<ICarSpecificationManager, CarSpecificationManager>();
            services.AddScoped<IVehicleSystemManager, VehicleSystemManager>();
            services.AddScoped<IVehicleTypeManager, VehicleTypeManager>();
            services.AddScoped<IVehicleUsageManager, VehicleUsageManager>();
            services.AddScoped<IProposalFormManager, ProposalFormManager>();
            services.AddScoped<ICarExteraDiscountManager, CarExteraDiscountManager>();
            services.AddScoped<ICarExteraDiscountValueManager, CarExteraDiscountValueManager>();
            services.AddScoped<ICompanyManager, CompanyManager>();
            services.AddScoped<ICarExteraDiscountRangeAmountManager, CarExteraDiscountRangeAmountManager>();
            services.AddScoped<ICarExteraDiscountCategoryManager, CarExteraDiscountCategoryManager>();
        }
    }
}
