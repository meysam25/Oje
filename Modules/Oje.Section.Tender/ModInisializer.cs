using Oje.Infrastructure;
using Oje.Infrastructure.Interfac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oje.Section.Tender.Interfaces;
using Oje.Section.Tender.Services;
using Oje.Section.Tender.Services.EContext;

namespace Oje.Section.Tender
{
    public class ModInisializer : IModInisializer
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<TenderDBContext>(options => options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"], b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)));

            services.AddScoped<ITenderConfigService, TenderConfigService>();
            services.AddScoped<ITenderProposalFormJsonConfigService, TenderProposalFormJsonConfigService>();
            services.AddScoped<IProposalFormService, ProposalFormService>();
            services.AddScoped<IProposalFormCategoryService, ProposalFormCategoryService>();
            services.AddScoped<ITenderFilledFormService, TenderFilledFormService>();
            services.AddScoped<ITenderFilledFormKeyService, TenderFilledFormKeyService>();
            services.AddScoped<ITenderFilledFormsValueService, TenderFilledFormsValueService>();
            services.AddScoped<ITenderFilledFormJsonService, TenderFilledFormJsonService>();
            services.AddScoped<ITenderFilledFormPFService, TenderFilledFormPFService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ITenderFilledFormPriceService, TenderFilledFormPriceService>();
            services.AddScoped<ITenderFilledFormIssueService, TenderFilledFormIssueService>();
            services.AddScoped<ITenderFileService, TenderFileService>();
            services.AddScoped<IUserRegisterFormService, UserRegisterFormService>();
            
        }
    }
}
