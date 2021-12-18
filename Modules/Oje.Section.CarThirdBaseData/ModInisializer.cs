using Oje.AccountService;
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

            services.AddScoped<ICarSpecificationService, CarSpecificationService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IThirdPartyRateService, ThirdPartyRateService>();
            services.AddScoped<IThirdPartyFinancialCommitmentService, ThirdPartyFinancialCommitmentService>();
            services.AddScoped<IThirdPartyLifeCommitmentService, ThirdPartyLifeCommitmentService>();
            services.AddScoped<IThirdPartyRequiredFinancialCommitmentService, ThirdPartyRequiredFinancialCommitmentService>();
            services.AddScoped<IThirdPartyExteraFinancialCommitmentService, ThirdPartyExteraFinancialCommitmentService>();
            services.AddScoped<IThirdPartyDriverFinancialCommitmentService, ThirdPartyDriverFinancialCommitmentService>();
            services.AddScoped<IThirdPartyPassengerRateService, ThirdPartyPassengerRateService>();
            services.AddScoped<IThirdPartyCarCreateDatePercentService, ThirdPartyCarCreateDatePercentService>();
            services.AddScoped<IThirdPartyDriverHistoryDamagePenaltyService, ThirdPartyDriverHistoryDamagePenaltyService>();
            services.AddScoped<IThirdPartyFinancialAndBodyHistoryDamagePenaltyService, ThirdPartyFinancialAndBodyHistoryDamagePenaltyService>();
            services.AddScoped<IThirdPartyDriverNoDamageDiscountHistoryService, ThirdPartyDriverNoDamageDiscountHistoryService>();
            services.AddScoped<IThirdPartyBodyNoDamageDiscountHistoryService, ThirdPartyBodyNoDamageDiscountHistoryService>();
        }
    }
}
