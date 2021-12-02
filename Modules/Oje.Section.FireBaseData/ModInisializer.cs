using Oje.AccountManager;
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

            services.AddScoped<IFireInsuranceCoverageTitleManager, FireInsuranceCoverageTitleManager>();
            services.AddScoped<IFireInsuranceCoverageManager, FireInsuranceCoverageManager>();
            services.AddScoped<ICompanyManager, CompanyManager>();
            services.AddScoped<IProposalFormManager, ProposalFormManager>();
            services.AddScoped<IFireInsuranceBuildingBodyManager, FireInsuranceBuildingBodyManager>();
            services.AddScoped<IFireInsuranceBuildingTypeManager, FireInsuranceBuildingTypeManager>();
            services.AddScoped<IFireInsuranceRateManager, FireInsuranceRateManager>();
            services.AddScoped<IFireInsuranceBuildingUnitValueManager, FireInsuranceBuildingUnitValueManager>();
            services.AddScoped<IFireInsuranceTypeOfActivityManager, FireInsuranceTypeOfActivityManager>();
            services.AddScoped<IFireInsuranceCoverageActivityDangerLevelManager, FireInsuranceCoverageActivityDangerLevelManager>();
            services.AddScoped<IFireInsuranceCoverageCityDangerLevelManager, FireInsuranceCoverageCityDangerLevelManager>();
            services.AddScoped<IFireInsuranceBuildingAgeManager, FireInsuranceBuildingAgeManager>();
        }
    }
}
