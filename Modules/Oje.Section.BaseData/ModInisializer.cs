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

            services.AddScoped<ISiteSettingManager, SiteSettingManager>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<ICompanyManager, CompanyManager>();
            services.AddScoped<IProposalFormManager, ProposalFormManager>();
            services.AddScoped<IProvinceManager, ProvinceManager>();
            services.AddScoped<ICityManager, CityManager>();
            services.AddScoped<IJobDangerLevelManager, JobDangerLevelManager>();
            services.AddScoped<IJobManager, JobManager>();
            services.AddScoped<ITaxManager, TaxManager>();
            services.AddScoped<IDutyManager, DutyManager>();
        }
    }
}
