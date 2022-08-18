using Oje.Infrastructure;
using Oje.Infrastructure.Interfac;
using Oje.Section.BaseData.Interfaces;
using Oje.Section.BaseData.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oje.Section.BaseData.Services.EContext;

namespace Oje.Section.BaseData
{
    public class ModInisializer : IModInisializer
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<BaseDataDBContext>(options =>
                    options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"],
                    b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery))
            );

            services.AddScoped<ISiteSettingService, SiteSettingService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IProposalFormService, ProposalFormService>();
            services.AddScoped<IProvinceService, ProvinceService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<IJobDangerLevelService, JobDangerLevelService>();
            services.AddScoped<IJobService, JobService>();
            services.AddScoped<ITaxService, TaxService>();
            services.AddScoped<IDutyService, DutyService>();
            services.AddScoped<IColorService, ColorService>();
        }
    }
}
