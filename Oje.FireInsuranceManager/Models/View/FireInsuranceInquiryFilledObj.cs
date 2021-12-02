using Oje.FireInsuranceManager.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceManager.Models.View
{
    public class FireInsuranceInquiryFilledObj
    {
        public FireInsuranceInquiryFilledObj()
        {
            Companies = new();
            GlobalDiscounts = new();
            CashPayDiscounts = new();
            FireInsuranceRates = new();
            InquiryMaxDiscounts = new();
            InqueryDescriptions = new();
            FireInsuranceCoverageTitles = new();
            FireInsuranceTypeOfActivities = new();
        }

        public long sarmaye { get; set; }
        public long arzeshSakhteman { get; set; }
        public decimal v100 { get; } = 100;

        public Province Province { get; set; }
        public City City { get; set; }
        public FireInsuranceBuildingUnitValue FireInsuranceBuildingUnitValue { get; set; }
        public FireInsuranceBuildingType FireInsuranceBuildingType { get; set; }
        public FireInsuranceBuildingBody FireInsuranceBuildingBody { get; set; }
        public FireInsuranceBuildingAge FireInsuranceBuildingAge { get; set; }
        public Tax Tax { get; set; }
        public Duty Duty { get; set; }
        public ProposalForm ProposalForm { get; set; }
        public List<InquiryMaxDiscount> InquiryMaxDiscounts { get; set; }
        public List<Company> Companies { get; set; }
        public InquiryDuration InquiryDuration { get; set; }
        public InsuranceContractDiscount InsuranceContractDiscount { get; set; }
        public List<GlobalDiscount> GlobalDiscounts { get; set; }
        public List<CashPayDiscount> CashPayDiscounts { get; set; }
        public List<PaymentMethod> PaymentMethods { get; set; }
        public List<FireInsuranceRate> FireInsuranceRates { get; set; }
        public RoundInquery RoundInquery { get; set; }
        public List<InqueryDescription> InqueryDescriptions { get; set; }
        public List<FireInsuranceCoverageTitle> FireInsuranceCoverageTitles { get; set; }
        public List<FireInsuranceTypeOfActivity> FireInsuranceTypeOfActivities { get; set; }
    }
}
