using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oje.Infrastructure;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Services;
using Oje.ProposalFormService.Services.EContext;

namespace Oje.ProposalFormService
{
    public static class ProposalFormConfig
    {
        public static void Config(IServiceCollection services)
        {
            services.AddDbContext<ProposalFormDBContext>(options => options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"], b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)));

            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IGlobalInputInqueryService, GlobalInputInqueryService>();
            services.AddScoped<IThirdPartyRateService, ThirdPartyRateService>();
            services.AddScoped<IGlobalInqueryService, GlobalInqueryService>();
            services.AddScoped<IInquiryCompanyLimitService, InquiryCompanyLimitService>();
            services.AddScoped<ICarExteraDiscountService, CarExteraDiscountService>();
            services.AddScoped<IProposalFormService, Services.ProposalFormService>();
            services.AddScoped<INoDamageDiscountService, NoDamageDiscountService>();
            services.AddScoped<IThirdPartyDriverNoDamageDiscountHistoryService, ThirdPartyDriverNoDamageDiscountHistoryService>();
            services.AddScoped<IVehicleTypeService, VehicleTypeService>();
            services.AddScoped<IVehicleSystemService, VehicleSystemService>();
            services.AddScoped<IVehicleUsageService, VehicleUsageService>();
            services.AddScoped<IThirdPartyFinancialAndBodyHistoryDamagePenaltyService, ThirdPartyFinancialAndBodyHistoryDamagePenaltyService>();
            services.AddScoped<IThirdPartyDriverHistoryDamagePenaltyService, ThirdPartyDriverHistoryDamagePenaltyService>();
            services.AddScoped<ICashPayDiscountService, CashPayDiscountService>();
            services.AddScoped<IThirdPartyFinancialCommitmentService, ThirdPartyFinancialCommitmentService>();
            services.AddScoped<IThirdPartyLifeCommitmentService, ThirdPartyLifeCommitmentService>();
            services.AddScoped<IThirdPartyDriverFinancialCommitmentService, ThirdPartyDriverFinancialCommitmentService>();
            services.AddScoped<IRoundInqueryService, RoundInqueryService>();
            services.AddScoped<ICarSpecificationService, CarSpecificationService>();
            services.AddScoped<ITaxService, TaxService>();
            services.AddScoped<IDutyService, DutyService>();
            services.AddScoped<IThirdPartyCarCreateDatePercentService, ThirdPartyCarCreateDatePercentService>();
            services.AddScoped<IInquiryMaxDiscountService, InquiryMaxDiscountService>();
            services.AddScoped<IGlobalDiscountService, GlobalDiscountService>();
            services.AddScoped<IThirdPartyRequiredFinancialCommitmentService, ThirdPartyRequiredFinancialCommitmentService>();
            services.AddScoped<IInsuranceContractDiscountService, InsuranceContractDiscountService>();
            services.AddScoped<IInquiryDurationService, InquiryDurationService>();
            services.AddScoped<IInqueryDescriptionService, InqueryDescriptionService>();
            services.AddScoped<IPaymentMethodService, PaymentMethodService>();
            services.AddScoped<IThirdPartyPassengerRateService, ThirdPartyPassengerRateService>();
            services.AddScoped<IThirdPartyBodyNoDamageDiscountHistoryService, ThirdPartyBodyNoDamageDiscountHistoryService>();
            services.AddScoped<IProposalFormRequiredDocumentService, ProposalFormRequiredDocumentService>();
            services.AddScoped<IProposalFilledFormService, ProposalFilledFormService>();
            services.AddScoped<IBankService, BankService>();
            services.AddScoped<IProposalFilledFormJsonService, ProposalFilledFormJsonService>();
            services.AddScoped<IProposalFilledFormCompanyService, ProposalFilledFormCompanyService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IProposalFilledFormUseService, ProposalFilledFormUseService>();
            services.AddScoped<IProposalFilledFormDocumentService, ProposalFilledFormDocumentService>();
            services.AddScoped<IProposalFilledFormValueService, ProposalFilledFormValueService>();
            services.AddScoped<IProposalFilledFormKeyService, ProposalFilledFormKeyService>();
            services.AddScoped<ICarSpecificationAmountService, CarSpecificationAmountService>();
            services.AddScoped<ICarBodyCreateDatePercentService, CarBodyCreateDatePercentService>();
            services.AddScoped<IProposalFilledFormAdminService, ProposalFilledFormAdminService>();
            services.AddScoped<IProposalFormCategoryService, ProposalFormCategoryService>();
            services.AddScoped<IProposalFilledFormAdminBaseQueryService, ProposalFilledFormAdminBaseQueryService>();
            services.AddScoped<IProposalFilledFormStatusLogService, ProposalFilledFormStatusLogService>();
        }
    }
}
