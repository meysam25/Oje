using Oje.AccountManager;
using Oje.Infrastructure;
using Oje.Infrastructure.Interfac;
using Oje.Section.SalesNetworkBaseData.Interfaces;
using Oje.Section.SalesNetworkBaseData.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oje.Section.SalesNetworkBaseData.Services.EContext;

namespace Oje.Section.SalesNetworkBaseData
{
    public class ModInisializer : IModInisializer
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<SalesNetworkBaseDataDBContext>(
               options => options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"], b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery))
           );

            services.AddScoped<ISalesNetworkManager, SalesNetworkManager>();
            services.AddScoped<ICompanyManager, CompanyManager>();
            services.AddScoped<IProposalFormManager, ProposalFormManager>();
        }
    }
}
