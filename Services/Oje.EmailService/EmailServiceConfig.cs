using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oje.EmailService.Interfaces;
using Oje.EmailService.Services;
using Oje.EmailService.Services.EContext;
using Oje.Infrastructure;

namespace Oje.EmailService
{
    public static class EmailServiceConfig
    {
        public static void Config(IServiceCollection services)
        {
            services.AddDbContextPool<EmailServiceDBContext>(options => options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"], b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)));

            services.AddScoped<IEmailTrigerService, EmailTrigerService>();
            services.AddScoped<IEmailTemplateService, EmailTemplateService>();
            services.AddScoped<IEmailSendingQueueService, EmailSendingQueueService>();
            services.AddScoped<IEmailSendingQueueErrorService, EmailSendingQueueErrorService>();
            services.AddScoped<IEmailConfigService, EmailConfigService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailSenderService, EmailSenderService>();
        }
    }
}
