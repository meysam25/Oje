using Microsoft.Extensions.DependencyInjection;
using Oje.Infrastructure.Interfac;
using Oje.JoinServices.Interfaces;
using Oje.JoinServices.Services;

namespace Oje.JoinServices
{
    public static class JoinServicesConfig
    {
        public static void Config(IServiceCollection services)
        {
            services.AddScoped<IUserNotifierService, UserNotifierService>();
            services.AddScoped<ISMSUserService, SMSUserService>();
        }
    }
}
