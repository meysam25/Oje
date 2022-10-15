using Oje.ProposalFormService.Models.DB;
using System.Collections.Generic;

namespace Oje.ProposalFormService.Models.View
{
    public class CarBodyInquiryObjects
    {
        public CarBodyInquiryObjects()
        {
            CarBodyCreateDatePercents = new();
            InquiryMaxDiscounts = new();
            GlobalDiscounts = new();
            CashPayDiscounts = new();
            InqueryDescriptions = new();
            PaymentMethods = new();
            CarSpecificationAmounts = new();
        }

        public decimal v100 { get { return 100; } }
        public ProposalForm currentProposalForm { get; set; }
        public List<Company> validCompanies { get; set; }
        public Company prevInsuranceCompany { get; set; }
        public List<CarExteraDiscount> requiredValidDynamicCTRLs { get; set; }
        public List<CarExteraDiscount> requiredSelectedDynamicCTRLs { get; set; }
        public List<CarExteraDiscount> rightOptionDynamicFilterCTRLs { get; set; }
        public VehicleType VehicleType { get; set; }
        public VehicleUsage VehicleUsage { get; set; }
        public RoundInquery RoundInquery { get; set; }
        public CarSpecification CarSpecification { get; set; }
        public Tax Tax { get; set; }
        public Duty Duty { get; set; }
        public NoDamageDiscount NoDamageDiscount { get; set; }
        public NoDamageDiscount NoDamageDiscountExtera { get; set; }
        public InsuranceContractDiscount InsuranceContractDiscount { get; set; }
        public InquiryDuration InquiryDuration { get; set; }


        public List<CarBodyCreateDatePercent> CarBodyCreateDatePercents { get; set; }
        public List<InquiryMaxDiscount> InquiryMaxDiscounts { get; set; }
        public List<GlobalDiscount> GlobalDiscounts { get; set; }
        public List<CashPayDiscount> CashPayDiscounts { get; set; }
        public List<InqueryDescription> InqueryDescriptions { get; set; }
        public List<PaymentMethod> PaymentMethods { get; set; }
        public List<CarSpecificationAmount> CarSpecificationAmounts { get; set; }
        public CarType CarType { get;  set; }
        public VehicleSpec VehicleSpec { get; set; }
    }
}
