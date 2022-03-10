using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oje.Infrastructure;
using Oje.Sms.Interfaces;
using Oje.Sms.Services;
using Oje.Sms.Services.EContext;

namespace Oje.Sms
{
    public static class SmsConfig
    {
        public static void Config(IServiceCollection services)
        {
            services.AddHttpClient();

            services.AddDbContext<SmsDBContext>(options => options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"], b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)));

            services.AddScoped<ISmsConfigService, SmsConfigService>();
            services.AddScoped<ISmsTrigerService, SmsTrigerService>();
            services.AddScoped<ISmsTemplateService, SmsTemplateService>();
            services.AddScoped<ISmsSendingQueueService, SmsSendingQueueService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IMagfaSMSSenderService, MagfaSMSSenderService>();
            services.AddScoped<ISmsSenderService, SmsSenderService>();
            services.AddScoped<ISmsSendingQueueErrorService, SmsSendingQueueErrorService>();
            services.AddScoped<ISmsValidationHistoryService, SmsValidationHistoryService>();

            cacheServices = services;
        }

        public static void ConfigForWorker(IServiceCollection services)
        {
            services.AddHttpClient();

            services.AddDbContext<SmsDBContext>(options => options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"], b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)), ServiceLifetime.Singleton);

            services.AddSingleton<ISmsConfigService, SmsConfigService>();
            services.AddSingleton<ISmsTrigerService, SmsTrigerService>();
            services.AddSingleton<ISmsTemplateService, SmsTemplateService>();
            services.AddSingleton<ISmsSendingQueueService, SmsSendingQueueService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IRoleService, RoleService>();
            services.AddSingleton<IMagfaSMSSenderService, MagfaSMSSenderService>();
            services.AddSingleton<ISmsSenderService, SmsSenderService>();
            services.AddSingleton<ISmsSendingQueueErrorService, SmsSendingQueueErrorService>();
            services.AddSingleton<ISmsValidationHistoryService, SmsValidationHistoryService>();

            cacheServices = services;
        }

        public static IServiceCollection cacheServices { get; set; }
    }
}
