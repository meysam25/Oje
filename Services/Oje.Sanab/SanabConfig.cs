using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oje.Infrastructure;
using Oje.Sanab.Interfaces;
using Oje.Sanab.Services;
using Oje.Sms.Services.EContext;

namespace Oje.Sanab
{
    public static class SmsConfig
    {
        public static void Config(IServiceCollection services)
        {
            services.AddHttpClient();

            services.AddDbContext<SanabDBContext>(options => options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"], b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)));

            services.AddScoped<ISanabUserService, SanabUserService>();
            services.AddScoped<ISanabLoginService, SanabLoginService>();
            services.AddScoped<ICarInquiry, CarInquiry>();
            services.AddScoped<IUserInquiry, UserInquiry>();
        }
    }
}
