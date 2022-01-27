using Oje.AccountService.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure;
using Oje.AccountService.Interfaces;
using Oje.AccountService.Services.EContext;
using Oje.FileService;

namespace Oje.AccountService
{
    public static class AccountConfig
    {
        public static void Config(IServiceCollection services)
        {
            FileServiceConfig.Config(services);

            services.AddDbContextPool<AccountDBContext>(options => options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"], b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)));

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISectionService, SectionService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ISiteSettingService, SiteSettingService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ISiteInfoService, SiteInfoService>();
            services.AddScoped<IProposalFormService, ProposalFormService>();
            services.AddScoped<IProvinceService, ProvinceService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<IUserNotificationService, UserNotificationService>();
            services.AddScoped<IUserNotificationTrigerService, UserNotificationTrigerService>();
            services.AddScoped<IUserNotificationTemplateService, UserNotificationTemplateService>();
            services.AddScoped<IDashboardSectionService, DashboardSectionService>();
            services.AddScoped<IActionService, ActionService>();
            services.AddScoped<IPropertyService, PropertyService>();
        }
    }
}
