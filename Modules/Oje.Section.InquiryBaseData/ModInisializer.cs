using Oje.Infrastructure;
using Oje.Infrastructure.Interfac;
using Oje.Section.InquiryBaseData.Interfaces;
using Oje.Section.InquiryBaseData.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oje.Section.InquiryBaseData.Services.EContext;

namespace Oje.Section.InquiryBaseData
{
    public class ModInisializer : IModInisializer
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<InquiryBaseDataDBContext>(
               options => options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"], b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery))
           );

            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IInquiryDurationService, InquiryDurationService>();
            services.AddScoped<IProposalFormService, ProposalFormService>();
            services.AddScoped<ICashPayDiscountService, CashPayDiscountService>();
            services.AddScoped<IInquiryMaxDiscountService, InquiryMaxDiscountService>();
            services.AddScoped<IGlobalDiscountService, GlobalDiscountService>();
            services.AddScoped<IInsuranceContractDiscountService, InsuranceContractDiscountService>();
            services.AddScoped<IInsuranceContractService, InsuranceContractService>();
            services.AddScoped<IInquiryCompanyLimitService, InquiryCompanyLimitService>();
            services.AddScoped<IRoundInqueryService, RoundInqueryService>();
            services.AddScoped<INoDamageDiscountService, NoDamageDiscountService>();
            services.AddScoped<IInqueryDescriptionService, InqueryDescriptionService>();
            services.AddScoped<ISiteSettingService, SiteSettingService>();
        }
    }
}
