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

            services.AddDbContextPool<AccountDBContext>(options => options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"], b => { b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery); b.UseNetTopologySuite(); }));

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
            services.AddScoped<IDashboardSectionCategoryService, DashboardSectionCategoryService>();
            services.AddScoped<ISectionCategoryService, SectionCategoryService>();
            services.AddScoped<IControllerCategoryService, ControllerCategoryService>();
            services.AddScoped<IControllerService, ControllerService>();
            services.AddScoped<IPushNotificationService, PushNotificationService>();
            services.AddScoped<IExternalNotificationServiceConfigService, ExternalNotificationServiceConfigService>();
            services.AddScoped<IExternalNotificationServicePushSubscriptionService, ExternalNotificationServicePushSubscriptionService>();
            services.AddScoped<IExternalNotificationServicePushSubscriptionErrorService, ExternalNotificationServicePushSubscriptionErrorService>();
            services.AddScoped<IWalletTransactionService, WalletTransactionService>();
            services.AddScoped<IUserMessageService, UserMessageService>();
            services.AddScoped<IUserMessageReplyService, UserMessageReplyService>();
            services.AddScoped<ITempSqlCommService, TempSqlCommService>();
            services.AddScoped<ILoginDescrptionService, LoginDescrptionService>();
            services.AddScoped<ILoginBackgroundImageService, LoginBackgroundImageService>();
            services.AddScoped<IHolydayService, HolydayService>();
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<IOurObjectService, OurObjectService>();
        }

        public static void ConfigForWorker(IServiceCollection services)
        {

            services.AddDbContext<AccountDBContext>(options => options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"], b => { b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery); b.UseNetTopologySuite(); }), ServiceLifetime.Singleton);

            services.AddSingleton<IPushNotificationService, PushNotificationService>();
            services.AddSingleton<IExternalNotificationServiceConfigService, ExternalNotificationServiceConfigService>();
            services.AddSingleton<IExternalNotificationServicePushSubscriptionService, ExternalNotificationServicePushSubscriptionService>();
            services.AddSingleton<IExternalNotificationServicePushSubscriptionErrorService, ExternalNotificationServicePushSubscriptionErrorService>();
        }
    }
}
