using Oje.AccountService;
using Oje.Infrastructure;
using Oje.Infrastructure.Interfac;
using Oje.Section.FireBaseData.Interfaces;
using Oje.Section.FireBaseData.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oje.Section.FireBaseData.Services.EContext;

namespace Oje.Section.FireBaseData
{
    public class ModInisializer : IModInisializer
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<FireBaseDataDBContext>(
                options => options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"], b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery))
            );

            services.AddScoped<IFireInsuranceCoverageTitleService, FireInsuranceCoverageTitleService>();
            services.AddScoped<IFireInsuranceCoverageService, FireInsuranceCoverageService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IProposalFormService, ProposalFormService>();
            services.AddScoped<IFireInsuranceBuildingBodyService, FireInsuranceBuildingBodyService>();
            services.AddScoped<IFireInsuranceBuildingTypeService, FireInsuranceBuildingTypeService>();
            services.AddScoped<IFireInsuranceRateService, FireInsuranceRateService>();
            services.AddScoped<IFireInsuranceBuildingUnitValueService, FireInsuranceBuildingUnitValueService>();
            services.AddScoped<IFireInsuranceTypeOfActivityService, FireInsuranceTypeOfActivityService>();
            services.AddScoped<IFireInsuranceCoverageActivityDangerLevelService, FireInsuranceCoverageActivityDangerLevelService>();
            services.AddScoped<IFireInsuranceCoverageCityDangerLevelService, FireInsuranceCoverageCityDangerLevelService>();
            services.AddScoped<IFireInsuranceBuildingAgeService, FireInsuranceBuildingAgeService>();
        }
    }
}
