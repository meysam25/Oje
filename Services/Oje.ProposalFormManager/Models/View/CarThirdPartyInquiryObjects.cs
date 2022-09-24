using Oje.ProposalFormService.Models.DB;
using System.Collections.Generic;

namespace Oje.ProposalFormService.Models.View
{
    public class CarThirdPartyInquiryObjects
    {
        public decimal v100 { get { return 100; } }
        public long ThirdPartyDriverFinancialCommitmentLong { get; set; }
        public long ThirdPartyLifeCommitmentLong { get; set; }
        public long ThirdPartyFinancialCommitmentLong { get; set; }
        public bool usMinus5ForNoDamageDiscount { get; set; }
        public ProposalForm currentProposalForm { get; set; }
        public List<Company> validCompanies { get; set; }
        public Company prevInsuranceCompany { get; set; }
        public List<CarExteraDiscount> requiredValidDynamicCTRLs { get; set; }
        public List<CarExteraDiscount> requiredSelectedDynamicCTRLs { get; set; }
        public List<CarExteraDiscount> rightOptionDynamicFilterCTRLs { get; set; }
        public ThirdPartyBodyNoDamageDiscountHistory bodyNoDamagePercent { get; set; }
        public ThirdPartyDriverNoDamageDiscountHistory ThirdPartyDriverNoDamageDiscountHistory { get; set; }
        public VehicleType VehicleType { get; set; }
        public VehicleUsage VehicleUsage { get; set; }
        public ThirdPartyFinancialAndBodyHistoryDamagePenalty ThirdPartyFinancialAndBodyHistoryDamagePenaltyNotFinancial { get; set; }
        public ThirdPartyFinancialAndBodyHistoryDamagePenalty ThirdPartyFinancialAndBodyHistoryDamagePenaltyFinancial { get; set; }
        public ThirdPartyDriverHistoryDamagePenalty ThirdPartyDriverHistoryDamagePenalty { get; set; }
        public ThirdPartyFinancialCommitment ThirdPartyFinancialCommitment { get; set; }
        public ThirdPartyLifeCommitment ThirdPartyLifeCommitment { get; set; }
        public ThirdPartyDriverFinancialCommitment ThirdPartyDriverFinancialCommitment { get; set; }
        public List<ThirdPartyRate> ThirdPartyRates { get; set; }
        public List<ThirdPartyPassengerRate> ThirdPartyPassengerRates { get; set; }
        public RoundInquery RoundInquery { get; set; }
        public CarSpecification CarSpecification { get; set; }
        public Tax Tax { get; set; }
        public Duty Duty { get; set; }
        public List<ThirdPartyCarCreateDatePercent> ThirdPartyCarCreateDatePercents { get; set; }
        public List<InquiryMaxDiscount> InquiryMaxDiscounts { get; set; }
        public List<GlobalDiscount> GlobalDiscounts { get; set; }
        public List<ThirdPartyRequiredFinancialCommitment> ThirdPartyRequiredFinancialCommitments { get; set; }
        public InsuranceContractDiscount InsuranceContractDiscount { get; set; }
        public InquiryDuration InquiryDuration { get; set; }
        public List<CashPayDiscount> CashPayDiscounts { get; set; }
        public List<InqueryDescription> InqueryDescriptions { get; set; }
        public List<PaymentMethod> PaymentMethods { get; set; }
        public CarType CarType { get;  set; }
        public VehicleSpec VehicleSpec { get; set; }
        public List<ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscount> ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscounts { get; set; }
    }
}
