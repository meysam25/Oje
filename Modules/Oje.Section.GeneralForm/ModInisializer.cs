using Oje.Infrastructure;
using Oje.Infrastructure.Interfac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oje.Section.GlobalForms.Services.EContext;
using Oje.Section.GlobalForms.Interfaces;
using Oje.Section.GlobalForms.Services;

namespace Oje.Section.GlobalForms
{
    public class ModInisializer : IModInisializer
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<GeneralFormDBContext>(options => options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"], b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)));

            services.AddScoped<IGeneralFormService, GeneralFormService>();
            services.AddScoped<ISiteSettingService, SiteSettingService>();
            services.AddScoped<IGeneralFormStatusService, GeneralFormStatusService>();
            services.AddScoped<IGeneralFormRequiredDocumentService, GeneralFormRequiredDocumentService>();
            services.AddScoped<IGeneralFilledFormService, GeneralFilledFormService>();
            services.AddScoped<IGeneralFormJsonService, GeneralFormJsonService>();
            services.AddScoped<IInternalUserService, InternalUserService>();
            services.AddScoped<IGeneralFilledFormValueService, GeneralFilledFormValueService>();
            services.AddScoped<IGeneralFilledFormKeyService, GeneralFilledFormKeyService>();
        }
    }
}
