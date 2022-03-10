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
            services.AddScoped<IBlockClientConfigService, BlockClientConfigService>();
            services.AddScoped<IBlockAutoIpService, BlockAutoIpService>();
            services.AddScoped<IBlockFirewallIpService, BlockFirewallIpService>();

            cacheServices = services;
        }

        public static void ConfigForWorker(IServiceCollection services)
        {
            services.AddDbContext<SecurityDBContext>(options =>
                    options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"],
                    b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery))
                    , ServiceLifetime.Singleton
            );

            services.AddSingleton<IIpLimitationWhiteListService, IpLimitationWhiteListService>();
            services.AddSingleton<IIpLimitationBlackListService, IpLimitationBlackListService>();
            services.AddSingleton<IFileAccessRoleService, FileAccessRoleService>();
            services.AddSingleton<IRoleService, RoleService>();
            services.AddSingleton<IBlockClientConfigService, BlockClientConfigService>();
            services.AddSingleton<IBlockAutoIpService, BlockAutoIpService>();
            services.AddSingleton<IBlockFirewallIpService, BlockFirewallIpService>();

            cacheServices = services;
        }


        public static IServiceCollection cacheServices { get; set; }
    }
}
