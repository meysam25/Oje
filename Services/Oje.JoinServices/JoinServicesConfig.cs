using Microsoft.Extensions.DependencyInjection;
using Oje.JoinServices.Interfaces;
using Oje.JoinServices.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
