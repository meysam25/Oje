using Oje.AccountManager;
using Oje.Infrastructure;
using Oje.Infrastructure.Interfac;
using Oje.Section.InsuranceContractBaseData.Interfaces;
using Oje.Section.InsuranceContractBaseData.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oje.Section.InsuranceContractBaseData.Services.EContext;

namespace Oje.Section.InsuranceContractBaseData
{
    public class ModInisializer : IModInisializer
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<InsuranceContractBaseDataDBContext>(
               options => options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"], b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery))
           );

            services.AddScoped<IInsuranceContractTypeManager, InsuranceContractTypeManager>();
            services.AddScoped<IInsuranceContractCompanyManager, InsuranceContractCompanyManager>();
            services.AddScoped<IInsuranceContractManager, InsuranceContractManager>();
            services.AddScoped<IProposalFormManager, ProposalFormManager>();
            services.AddScoped<IInsuranceContractValidUserForFullDebitManager, InsuranceContractValidUserForFullDebitManager>();
            services.AddScoped<IInsuranceContractUserManager, InsuranceContractUserManager>();
        }
    }
}
