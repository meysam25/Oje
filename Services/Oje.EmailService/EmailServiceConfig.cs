using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oje.EmailService.Interfaces;
using Oje.EmailService.Services;
using Oje.EmailService.Services.EContext;
using Oje.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.EmailService
{
    public static class EmailServiceConfig
    {
        public static void Config(IServiceCollection services)
        {
            services.AddDbContext<EmailServiceDBContext>(options => options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"], b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)));

            services.AddScoped<IEmailTrigerService, EmailTrigerService>();
            services.AddScoped<IEmailTemplateService, EmailTemplateService>();
            services.AddScoped<IEmailSendingQueueService, EmailSendingQueueService>();
            services.AddScoped<IEmailSendingQueueErrorService, EmailSendingQueueErrorService>();
            services.AddScoped<IEmailConfigService, EmailConfigService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailSenderService, EmailSenderService>();

            cacheServices = services;
        }

        public static IServiceCollection cacheServices { get; set; }
    }
}
