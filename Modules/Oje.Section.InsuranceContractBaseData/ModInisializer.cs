using Oje.AccountService;
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

            services.AddScoped<IInsuranceContractTypeService, InsuranceContractTypeService>();
            services.AddScoped<IInsuranceContractCompanyService, InsuranceContractCompanyService>();
            services.AddScoped<IInsuranceContractService, InsuranceContractService>();
            services.AddScoped<IProposalFormService, ProposalFormService>();
            services.AddScoped<IInsuranceContractValidUserForFullDebitService, InsuranceContractValidUserForFullDebitService>();
            services.AddScoped<IInsuranceContractUserService, InsuranceContractUserService>();
        }
    }
}
