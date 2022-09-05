using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oje.FileService.Interfaces;
using Oje.FileService.Services;
using Oje.FileService.Services.EContext;
using Oje.Infrastructure;

namespace Oje.FileService
{
    public static class FileServiceConfig
    {
        public static void Config(IServiceCollection services)
        {
            services.AddDbContextPool<FileDBContext>(options => options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"], b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)));

            services.AddScoped<IUploadedFileService, UploadedFileService>();
        }
    }
}
