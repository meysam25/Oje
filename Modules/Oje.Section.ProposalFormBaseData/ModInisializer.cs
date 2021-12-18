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

            services.AddScoped<IProposalFormPostPriceService, ProposalFormPostPriceService>();
            services.AddScoped<IProposalFormService, ProposalFormService>();
            services.AddScoped<ISiteSettingService, SiteSettingService>();
            services.AddScoped<IProposalFormCategoryService, ProposalFormCategoryService>();
            services.AddScoped<IProposalFormRequiredDocumentService, ProposalFormRequiredDocumentService>();
            services.AddScoped<IProposalFormRequiredDocumentTypeService, ProposalFormRequiredDocumentTypeService>();
            services.AddScoped<IPaymentMethodService, PaymentMethodService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IPaymentMethodFileService, PaymentMethodFileService>();
        }
    }
}
