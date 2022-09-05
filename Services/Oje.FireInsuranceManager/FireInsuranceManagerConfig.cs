using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oje.FireInsuranceService.Interfaces;
using Oje.FireInsuranceService.Services;
using Oje.FireInsuranceService.Services.EContext;
using Oje.Infrastructure;

namespace Oje.FireInsuranceService
{
    public static class FireInsuranceServiceConfig
    {
        public static void Config(IServiceCollection services)
        {
            services.AddDbContextPool<FireInsuranceServiceDBContext>(options => options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"], b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)));

            services.AddScoped<IFireInsuranceBuildingUnitValueService, FireInsuranceBuildingUnitValueService>();
            services.AddScoped<IFireInsuranceBuildingTypeService, FireInsuranceBuildingTypeService>();
            services.AddScoped<IFireInsuranceBuildingBodyService, FireInsuranceBuildingBodyService>();
            services.AddScoped<IFireInsuranceBuildingAgeService, FireInsuranceBuildingAgeService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IInquiryDurationService, InquiryDurationService>();
            services.AddScoped<IProposalFormService, ProposalFormService>();
            services.AddScoped<IInsuranceContractDiscountService, InsuranceContractDiscountService>();
            services.AddScoped<IFireInsuranceRateService, FireInsuranceRateService>();
            services.AddScoped<IProvinceService, ProvinceService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<ITaxService, TaxService>();
            services.AddScoped<IDutyService, DutyService>();
            services.AddScoped<IInquiryMaxDiscountService, InquiryMaxDiscountService>();
            services.AddScoped<IInquiryCompanyLimitService, InquiryCompanyLimitService>();
            services.AddScoped<IGlobalDiscountService, GlobalDiscountService>();
            services.AddScoped<ICashPayDiscountService, CashPayDiscountService>();
            services.AddScoped<IPaymentMethodService, PaymentMethodService>();
            services.AddScoped<IGlobalInputInqueryService, GlobalInputInqueryService>();
            services.AddScoped<IRoundInqueryService, RoundInqueryService>();
            services.AddScoped<IGlobalInqueryService, GlobalInqueryService>();
            services.AddScoped<IInqueryDescriptionService, InqueryDescriptionService>();
            services.AddScoped<IFireInsuranceCoverageTitleService, FireInsuranceCoverageTitleService>();
            services.AddScoped<IFireInsuranceTypeOfActivityService, FireInsuranceTypeOfActivityService>();
            services.AddScoped<IFireInsuranceCoverageService, FireInsuranceCoverageService>();
            services.AddScoped<IFireInsuranceCoverageCityDangerLevelService, FireInsuranceCoverageCityDangerLevelService>();
        }
    }
}
