using Oje.Infrastructure;
using Oje.Infrastructure.Interfac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oje.Section.Security.Services.EContext;
using Oje.Section.Security.Interfaces;
using Oje.Section.Security.Services;

namespace Oje.Section.Security
{
    public class ModInisializer : IModInisializer
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<SecurityDBContext>(options =>
                    options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"],
                    b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery))
            );

            services.AddScoped<IIpLimitationWhiteListManager, IpLimitationWhiteListManager>();
            services.AddScoped<IIpLimitationBlackListManager, IpLimitationBlackListManager>();
            services.AddScoped<IFileAccessRoleManager, FileAccessRoleManager>();
            services.AddScoped<IRoleManager, RoleManager>();

        }
    }
}
