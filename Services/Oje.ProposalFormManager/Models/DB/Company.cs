using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("Companies")]
    public class Company
    {
        public Company()
        {
            GlobalInqueries = new();
            ProposalFilledFormCompanies = new();
            CarExteraDiscountRangeAmountCompanies = new();
            NoDamageDiscountCompanies = new();
            ThirdPartyPassengerRateCompanies = new();
            ThirdPartyExteraFinancialCommitmentComs = new();
            ThirdPartyRequiredFinancialCommitmentCompanies = new();
            InsuranceContractDiscountCompanies = new();
            InquiryDurationCompanies = new();
            InqueryDescriptionCompanies = new();
            InquiryMaxDiscountCompanies = new();
            PaymentMethodCompanies = new();
            CarSpecificationAmountCompanies = new();
            UserCompanies = new();
            AgentReffers = new();
            ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountCompanies = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        [Required, MaxLength(100)]
        public string Pic { get; set; }
        [MaxLength(100)]
        public string Pic32 { get; set; }
        [MaxLength(100)]
        public string Pic64 { get; set; }
        public bool IsActive { get; set; }

        [InverseProperty("Company")]
        public List<GlobalInquery> GlobalInqueries { get; set; }
        [InverseProperty("Company")]
        public List<ProposalFilledFormCompany> ProposalFilledFormCompanies { get; set; }
        [InverseProperty("Company")]
        public List<CarExteraDiscountRangeAmountCompany> CarExteraDiscountRangeAmountCompanies { get; set; }
        [InverseProperty("Company")]
        public List<NoDamageDiscountCompany> NoDamageDiscountCompanies { get; set; }
        [InverseProperty("Company")]
        public List<CashPayDiscountCompany> CashPayDiscountCompanies { get; set; }
        [InverseProperty("Company")]
        public List<ThirdPartyPassengerRateCompany> ThirdPartyPassengerRateCompanies { get; set; }
        [InverseProperty("Company")]
        public List<GlobalDiscountCompany> GlobalDiscountCompanies { get; set; }
        [InverseProperty("Company")]
        public List<ThirdPartyExteraFinancialCommitmentCom> ThirdPartyExteraFinancialCommitmentComs { get; set; }
        [InverseProperty("Company")]
        public List<ThirdPartyRequiredFinancialCommitmentCompany> ThirdPartyRequiredFinancialCommitmentCompanies { get; set; }
        [InverseProperty("Company")]
        public List<InsuranceContractDiscountCompany> InsuranceContractDiscountCompanies { get; set; }
        [InverseProperty("Company")]
        public List<InquiryDurationCompany> InquiryDurationCompanies { get; set; }
        [InverseProperty("Company")]
        public List<InqueryDescriptionCompany> InqueryDescriptionCompanies { get; set; }
        [InverseProperty("Company")]
        public List<InquiryMaxDiscountCompany> InquiryMaxDiscountCompanies { get; set; }
        [InverseProperty("Company")]
        public List<PaymentMethodCompany> PaymentMethodCompanies { get; set; }
        [InverseProperty("Company")]
        public List<InquiryCompanyLimitCompany> InquiryCompanyLimitCompanies { get; set; }
        [InverseProperty("Company")]
        public List<CarSpecificationAmountCompany> CarSpecificationAmountCompanies { get; set; }
        [InverseProperty("Company")]
        public List<UserCompany> UserCompanies { get; set; }
        [InverseProperty("Company")]
        public List<AgentReffer> AgentReffers { get; set; }
        [InverseProperty("Company")]
        public List<ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountCompany> ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountCompanies { get; set; }

    }
}
