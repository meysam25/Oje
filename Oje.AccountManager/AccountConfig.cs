using Oje.AccountManager.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure;
using Oje.AccountManager.Interfaces;
using Oje.AccountManager.Services.EContext;

namespace Oje.AccountManager
{
    public static class AccountConfig
    {
        public static void Config(IServiceCollection services)
        {
            services.AddDbContextPool<AccountDBContext>(options =>
                    options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"],
                    b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery))
            );

            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<ISectionManager, SectionManager>();
            services.AddScoped<IRoleManager, RoleManager>();
            services.AddScoped<IUploadedFileManager, UploadedFileManager>();
            services.AddScoped<ISiteSettingManager, SiteSettingManager>();
            services.AddScoped<ICompanyManager, CompanyManager>();
            services.AddScoped<ISiteInfoManager, SiteInfoManager>();
            services.AddScoped<IProposalFormManager, ProposalFormManager>();
            services.AddScoped<IProvinceService, ProvinceService>();
            services.AddScoped<ICityManager, CityManager>();
        }
    }
}
