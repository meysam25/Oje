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

            services.AddScoped<ICarTypeService, CarTypeService>();
            services.AddScoped<ICarSpecificationService, CarSpecificationService>();
            services.AddScoped<IVehicleSystemService, VehicleSystemService>();
            services.AddScoped<IVehicleTypeService, VehicleTypeService>();
            services.AddScoped<IVehicleUsageService, VehicleUsageService>();
            services.AddScoped<IProposalFormService, ProposalFormService>();
            services.AddScoped<ICarExteraDiscountService, CarExteraDiscountService>();
            services.AddScoped<ICarExteraDiscountValueService, CarExteraDiscountValueService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ICarExteraDiscountRangeAmountService, CarExteraDiscountRangeAmountService>();
            services.AddScoped<ICarExteraDiscountCategoryService, CarExteraDiscountCategoryService>();
        }
    }
}
