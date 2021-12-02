using Oje.Infrastructure;
using Oje.Infrastructure.Interfac;
using Oje.Section.ProposalFormBaseData.Interfaces;
using Oje.Section.ProposalFormBaseData.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oje.Section.ProposalFormBaseData.Services.EContext;

namespace Oje.Section.ProposalFormBaseData
{
    public class ModInisializer : IModInisializer
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<ProposalFormBaseDataDBContext>(
               options => options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"], b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery))
           );

            services.AddScoped<IProposalFormPostPriceManager, ProposalFormPostPriceManager>();
            services.AddScoped<IProposalFormManager, ProposalFormManager>();
            services.AddScoped<ISiteSettingManager, SiteSettingManager>();
            services.AddScoped<IProposalFormCategoryManager, ProposalFormCategoryManager>();
            services.AddScoped<IProposalFormRequiredDocumentManager, ProposalFormRequiredDocumentManager>();
            services.AddScoped<IProposalFormRequiredDocumentTypeManager, ProposalFormRequiredDocumentTypeManager>();
            services.AddScoped<IPaymentMethodManager, PaymentMethodManager>();
            services.AddScoped<ICompanyManager, CompanyManager>();
            services.AddScoped<IPaymentMethodFileManager, PaymentMethodFileManager>();
        }
    }
}
