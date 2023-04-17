using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oje.Infrastructure;
using Oje.Sanab.Interfaces;
using Oje.Sanab.Services;
using Oje.Sms.Services.EContext;

namespace Oje.Sanab
{
    public static class SanabConfig
    {
        public static void Config(IServiceCollection services)
        {

            services.AddDbContextPool<SanabDBContext>(options => options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"], b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)));

            services.AddScoped<ISanabUserService, SanabUserService>();
            services.AddScoped<ISanabLoginService, SanabLoginService>();
            services.AddScoped<ICarInquiry, CarInquiry>();
            services.AddScoped<IUserInquiry, UserInquiry>();
            services.AddScoped<ISanabSystemFieldVehicleSystemService, SanabSystemFieldVehicleSystemService>();
            services.AddScoped<IVehicleSystemService, VehicleSystemService>();
            services.AddScoped<IVehicleSpecService, VehicleSpecService>();
            services.AddScoped<ISanabTypeFieldVehicleSpecService, SanabTypeFieldVehicleSpecService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ISanabCompanyService, SanabCompanyService>();
            services.AddScoped<ISanabCarThirdPartyPlaqueInquiryService, SanabCarThirdPartyPlaqueInquiryService>();
            services.AddScoped<IVehicleTypeService, VehicleTypeService>();
            services.AddScoped<ISanabVehicleTypeService, SanabVehicleTypeService>();
            services.AddScoped<ICarTypeService, CarTypeService>();
            services.AddScoped<ISanabCarTypeService, SanabCarTypeService>();
            services.AddScoped<IThirdPartyBodyNoDamageDiscountHistoryService, ThirdPartyBodyNoDamageDiscountHistoryService>();
            services.AddScoped<IThirdPartyDriverHistoryDamagePenaltyService, ThirdPartyDriverHistoryDamagePenaltyService>();
            services.AddScoped<IThirdPartyDriverNoDamageDiscountHistoryService, ThirdPartyDriverNoDamageDiscountHistoryService>();
            services.AddScoped<IThirdPartyFinancialAndBodyHistoryDamagePenaltyService, ThirdPartyFinancialAndBodyHistoryDamagePenaltyService>();
        }
    }
}
