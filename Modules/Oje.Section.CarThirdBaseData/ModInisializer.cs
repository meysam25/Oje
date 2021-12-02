using Oje.AccountManager;
using Oje.Infrastructure;
using Oje.Infrastructure.Interfac;
using Oje.Section.CarThirdBaseData.Interfaces;
using Oje.Section.CarThirdBaseData.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oje.Section.CarThirdBaseData.Services.EContext;

namespace Oje.Section.CarThirdBaseData
{
    public class ModInisializer : IModInisializer
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<CarThirdBaseDataDBContext>(
                options => options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"], b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery))
                );

            services.AddScoped<ICarSpecificationManager, CarSpecificationManager>();
            services.AddScoped<ICompanyManager, CompanyManager>();
            services.AddScoped<IThirdPartyRateManager, ThirdPartyRateManager>();
            services.AddScoped<IThirdPartyFinancialCommitmentManager, ThirdPartyFinancialCommitmentManager>();
            services.AddScoped<IThirdPartyLifeCommitmentManager, ThirdPartyLifeCommitmentManager>();
            services.AddScoped<IThirdPartyRequiredFinancialCommitmentManager, ThirdPartyRequiredFinancialCommitmentManager>();
            services.AddScoped<IThirdPartyExteraFinancialCommitmentManager, ThirdPartyExteraFinancialCommitmentManager>();
            services.AddScoped<IThirdPartyDriverFinancialCommitmentManager, ThirdPartyDriverFinancialCommitmentManager>();
            services.AddScoped<IThirdPartyPassengerRateManager, ThirdPartyPassengerRateManager>();
            services.AddScoped<IThirdPartyCarCreateDatePercentManager, ThirdPartyCarCreateDatePercentManager>();
            services.AddScoped<IThirdPartyDriverHistoryDamagePenaltyManager, ThirdPartyDriverHistoryDamagePenaltyManager>();
            services.AddScoped<IThirdPartyFinancialAndBodyHistoryDamagePenaltyManager, ThirdPartyFinancialAndBodyHistoryDamagePenaltyManager>();
            services.AddScoped<IThirdPartyDriverNoDamageDiscountHistoryManager, ThirdPartyDriverNoDamageDiscountHistoryManager>();
            services.AddScoped<IThirdPartyBodyNoDamageDiscountHistoryManager, ThirdPartyBodyNoDamageDiscountHistoryManager>();
        }
    }
}
