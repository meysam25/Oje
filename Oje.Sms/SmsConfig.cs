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
            services.AddDbContext<SmsDBContext>(options => options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"], b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)));


            services.AddScoped<ISmsConfigService, SmsConfigService>();
            services.AddScoped<ISmsTrigerService, SmsTrigerService>();
            services.AddScoped<ISmsTemplateService, SmsTemplateService>();
            services.AddScoped<ISmsSendingQueueService, SmsSendingQueueService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
        }
    }
}
