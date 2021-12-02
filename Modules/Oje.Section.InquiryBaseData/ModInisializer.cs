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

            services.AddScoped<ICompanyManager, CompanyManager>();
            services.AddScoped<IInquiryDurationManager, InquiryDurationManager>();
            services.AddScoped<IProposalFormManager, ProposalFormManager>();
            services.AddScoped<ICashPayDiscountManager, CashPayDiscountManager>();
            services.AddScoped<IInquiryMaxDiscountManager, InquiryMaxDiscountManager>();
            services.AddScoped<IGlobalDiscountManager, GlobalDiscountManager>();
            services.AddScoped<IInsuranceContractDiscountManager, InsuranceContractDiscountManager>();
            services.AddScoped<IInsuranceContractManager, InsuranceContractManager>();
            services.AddScoped<IInquiryCompanyLimitManager, InquiryCompanyLimitManager>();
            services.AddScoped<IRoundInqueryManager, RoundInqueryManager>();
            services.AddScoped<INoDamageDiscountManager, NoDamageDiscountManager>();
            services.AddScoped<IInqueryDescriptionManager, InqueryDescriptionManager>();
            services.AddScoped<ISiteSettingManager, SiteSettingManager>();
        }
    }
}
