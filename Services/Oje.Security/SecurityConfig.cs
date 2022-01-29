using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oje.Infrastructure;
using Oje.Security.Interfaces;
using Oje.Security.Services;
using Oje.Security.Services.EContext;

namespace Oje.Security
{
    public static class SecurityConfig
    {
        public static void Config(IServiceCollection services)
        {
            services.AddDbContextPool<SecurityDBContext>(options =>
                    options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"],
                    b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery))
            );

            services.AddScoped<IIpLimitationWhiteListService, IpLimitationWhiteListService>();
            services.AddScoped<IIpLimitationBlackListService, IpLimitationBlackListService>();
            services.AddScoped<IFileAccessRoleService, FileAccessRoleService>();
            services.AddScoped<IRoleService, RoleService>();
        }
    }
}
