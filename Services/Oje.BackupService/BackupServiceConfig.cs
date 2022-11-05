using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oje.BackupService.Interfaces;
using Oje.BackupService.Services;
using Oje.EmailService.Services.EContext;
using Oje.Infrastructure;

namespace Oje.BackupService
{
    public static class BackupServiceConfig
    {
        public static void Config(IServiceCollection services)
        {
            services.AddDbContext<BackupServiceDBContext>(options => options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"], b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)), ServiceLifetime.Singleton);

            services.AddSingleton<IFileBackupService, FileBackupService>();
            services.AddSingleton<IGoogleBackupService, GoogleBackupService>();
            services.AddSingleton<ISqlService, SqlService>();
            services.AddSingleton<IGoogleBackupArchiveService, GoogleBackupArchiveService>();
            services.AddSingleton<IGoogleBackupArchiveLogService, GoogleBackupArchiveLogService>();
            services.AddSingleton<IFileHelperService, FileHelperService>();
            services.AddSingleton<IMegaService, MegaService>();
        }
    }
}
