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
            services.AddScoped<IUserLoginConfigService, UserLoginConfigService>();
            services.AddScoped<IUserAdminLogConfigService, UserAdminLogConfigService>();
            services.AddScoped<IActionService, ActionService>();
            services.AddScoped<IUserAdminLogService, UserAdminLogService>();
            services.AddScoped<IErrorService, ErrorService>();
            services.AddScoped<IAdminBlockClientConfigService, AdminBlockClientConfigService>();
            services.AddScoped<IUserLoginLogoutLogService, UserLoginLogoutLogService>();
            services.AddScoped<IBlockLoginUserService, BlockLoginUserService>();
            services.AddScoped<IDebugEmailService, DebugEmailService>();
            services.AddScoped<IDebugEmailReceiverService, DebugEmailReceiverService>();
            services.AddScoped<IErrorFirewallManualAddService, ErrorFirewallManualAddService>();
            services.AddScoped<IValidRangeIpService, ValidRangeIpService>();
            services.AddScoped<IInValidRangeIpService, InValidRangeIpService>();
            services.AddScoped<IIpapiService, IpapiService>();
            services.AddScoped<IDebugInfoService, DebugInfoService>();
            services.AddScoped<IGoogleBackupArchiveService, GoogleBackupArchiveService>();
            services.AddScoped<IGoogleBackupArchiveLogService, GoogleBackupArchiveLogService>();
        }

        public static void ConfigForWorker(IServiceCollection services)
        {
            services.AddDbContext<SecurityDBContext>(options =>
                    options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"],
                    b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery))
                    , ServiceLifetime.Singleton
            );

            services.AddSingleton<IDebugEmailService, DebugEmailService>();
            services.AddSingleton<IDebugEmailReceiverService, DebugEmailReceiverService>();
            services.AddSingleton<IValidRangeIpService, ValidRangeIpService>();
            services.AddSingleton<IInValidRangeIpService, InValidRangeIpService>();
            services.AddSingleton<IIpapiService, IpapiService>();
        }
    }
}
