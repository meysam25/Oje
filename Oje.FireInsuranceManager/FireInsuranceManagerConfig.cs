using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oje.FireInsuranceManager.Interfaces;
using Oje.FireInsuranceManager.Services;
using Oje.FireInsuranceManager.Services.EContext;
using Oje.Infrastructure;

namespace Oje.FireInsuranceManager
{
    public static class FireInsuranceManagerConfig
    {
        public static void Config(IServiceCollection services)
        {
            services.AddDbContext<FireInsuranceManagerDBContext>(options => options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"], b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)));

            services.AddScoped<IFireInsuranceBuildingUnitValueManager, FireInsuranceBuildingUnitValueManager>();
            services.AddScoped<IFireInsuranceBuildingTypeManager, FireInsuranceBuildingTypeManager>();
            services.AddScoped<IFireInsuranceBuildingBodyManager, FireInsuranceBuildingBodyManager>();
            services.AddScoped<IFireInsuranceBuildingAgeManager, FireInsuranceBuildingAgeManager>();
            services.AddScoped<ICompanyManager, CompanyManager>();
            services.AddScoped<IInquiryDurationManager, InquiryDurationManager>();
            services.AddScoped<IProposalFormManager, ProposalFormManager>();
            services.AddScoped<IInsuranceContractDiscountManager, InsuranceContractDiscountManager>();
            services.AddScoped<IFireInsuranceRateManager, FireInsuranceRateManager>();
            services.AddScoped<IProvinceManager, ProvinceManager>();
            services.AddScoped<ICityManager, CityManager>();
            services.AddScoped<ITaxManager, TaxManager>();
            services.AddScoped<IDutyManager, DutyManager>();
            services.AddScoped<IInquiryMaxDiscountManager, InquiryMaxDiscountManager>();
            services.AddScoped<IInquiryCompanyLimitManager, InquiryCompanyLimitManager>();
            services.AddScoped<IGlobalDiscountManager, GlobalDiscountManager>();
            services.AddScoped<ICashPayDiscountManager, CashPayDiscountManager>();
            services.AddScoped<IPaymentMethodManager, PaymentMethodManager>();
            services.AddScoped<IGlobalInputInqueryManager, GlobalInputInqueryManager>();
            services.AddScoped<IRoundInqueryManager, RoundInqueryManager>();
            services.AddScoped<IGlobalInqueryManager, GlobalInqueryManager>();
            services.AddScoped<IInqueryDescriptionManager, InqueryDescriptionManager>();
            services.AddScoped<IFireInsuranceCoverageTitleManager, FireInsuranceCoverageTitleManager>();
            services.AddScoped<IFireInsuranceTypeOfActivityManager, FireInsuranceTypeOfActivityManager>();
            services.AddScoped<IFireInsuranceCoverageManager, FireInsuranceCoverageManager>();
            services.AddScoped<IFireInsuranceCoverageCityDangerLevelManager, FireInsuranceCoverageCityDangerLevelManager>();
        }
    }
}
