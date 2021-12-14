using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oje.Infrastructure;
using Oje.ProposalFormManager.Interfaces;
using Oje.ProposalFormManager.Services;
using Oje.ProposalFormManager.Services.EContext;

namespace Oje.ProposalFormManager
{
    public static class ProposalFormConfig
    {
        public static void Config(IServiceCollection services)
        {
            services.AddDbContext<ProposalFormDBContext>(options => options.UseSqlServer(GlobalConfig.Configuration["ConnectionStrings:DefaultConnection"], b => b.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)));

            services.AddScoped<ICompanyManager, CompanyManager>();
            services.AddScoped<IGlobalInputInqueryManager, GlobalInputInqueryManager>();
            services.AddScoped<IThirdPartyRateManager, ThirdPartyRateManager>();
            services.AddScoped<IGlobalInqueryManager, GlobalInqueryManager>();
            services.AddScoped<IInquiryCompanyLimitManager, InquiryCompanyLimitManager>();
            services.AddScoped<ICarExteraDiscountManager, CarExteraDiscountManager>();
            services.AddScoped<IProposalFormManager, Services.ProposalFormManager>();
            services.AddScoped<INoDamageDiscountManager, NoDamageDiscountManager>();
            services.AddScoped<IThirdPartyDriverNoDamageDiscountHistoryManager, ThirdPartyDriverNoDamageDiscountHistoryManager>();
            services.AddScoped<IVehicleTypeManager, VehicleTypeManager>();
            services.AddScoped<IVehicleSystemManager, VehicleSystemManager>();
            services.AddScoped<IVehicleUsageManager, VehicleUsageManager>();
            services.AddScoped<IThirdPartyFinancialAndBodyHistoryDamagePenaltyManager, ThirdPartyFinancialAndBodyHistoryDamagePenaltyManager>();
            services.AddScoped<IThirdPartyDriverHistoryDamagePenaltyManager, ThirdPartyDriverHistoryDamagePenaltyManager>();
            services.AddScoped<ICashPayDiscountManager, CashPayDiscountManager>();
            services.AddScoped<IThirdPartyFinancialCommitmentManager, ThirdPartyFinancialCommitmentManager>();
            services.AddScoped<IThirdPartyLifeCommitmentManager, ThirdPartyLifeCommitmentManager>();
            services.AddScoped<IThirdPartyDriverFinancialCommitmentManager, ThirdPartyDriverFinancialCommitmentManager>();
            services.AddScoped<IRoundInqueryManager, RoundInqueryManager>();
            services.AddScoped<ICarSpecificationManager, CarSpecificationManager>();
            services.AddScoped<ITaxManager, TaxManager>();
            services.AddScoped<IDutyManager, DutyManager>();
            services.AddScoped<IThirdPartyCarCreateDatePercentManager, ThirdPartyCarCreateDatePercentManager>();
            services.AddScoped<IInquiryMaxDiscountManager, InquiryMaxDiscountManager>();
            services.AddScoped<IGlobalDiscountManager, GlobalDiscountManager>();
            services.AddScoped<IThirdPartyRequiredFinancialCommitmentManager, ThirdPartyRequiredFinancialCommitmentManager>();
            services.AddScoped<IInsuranceContractDiscountManager, InsuranceContractDiscountManager>();
            services.AddScoped<IInquiryDurationManager, InquiryDurationManager>();
            services.AddScoped<IInqueryDescriptionManager, InqueryDescriptionManager>();
            services.AddScoped<IPaymentMethodManager, PaymentMethodManager>();
            services.AddScoped<IThirdPartyPassengerRateManager, ThirdPartyPassengerRateManager>();
            services.AddScoped<IThirdPartyBodyNoDamageDiscountHistoryManager, ThirdPartyBodyNoDamageDiscountHistoryManager>();
            services.AddScoped<IProposalFormRequiredDocumentManager, ProposalFormRequiredDocumentManager>();
            services.AddScoped<IProposalFilledFormManager, ProposalFilledFormManager>();
            services.AddScoped<IBankManager, BankManager>();
            services.AddScoped<IProposalFilledFormJsonManager, ProposalFilledFormJsonManager>();
            services.AddScoped<IProposalFilledFormCompanyManager, ProposalFilledFormCompanyManager>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IRoleManager, RoleManager>();
            services.AddScoped<IProposalFilledFormUseManager, ProposalFilledFormUseManager>();
            services.AddScoped<IProposalFilledFormDocumentManager, ProposalFilledFormDocumentManager>();
            services.AddScoped<IProposalFilledFormValueManager, ProposalFilledFormValueManager>();
            services.AddScoped<IProposalFilledFormKeyManager, ProposalFilledFormKeyManager>();
            services.AddScoped<ICarSpecificationAmountManager, CarSpecificationAmountManager>();
            services.AddScoped<ICarBodyCreateDatePercentManager, CarBodyCreateDatePercentManager>();
            services.AddScoped<IProposalFilledFormAdminManager, ProposalFilledFormAdminManager>();
            services.AddScoped<IProposalFormCategoryManager, ProposalFormCategoryManager>();
            services.AddScoped<IProposalFilledFormAdminBaseQueryManager, ProposalFilledFormAdminBaseQueryManager>();
            services.AddScoped<IProposalFilledFormStatusLogManager, ProposalFilledFormStatusLogManager>();
        }
    }
}
