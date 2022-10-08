using Oje.Infrastructure.Models;

namespace Oje.Section.InsuranceContractBaseData.Models.View
{
    public class CreateUpdateInsuranceContractValidUserForFullDebitVM: GlobalSiteSetting
    {
        public long? id { get; set; }
        public int? insuranceContractId { get; set; }
        public string mobile { get; set; }
        public string nationalCode { get; set; }
        public int? countUse { get; set; }
        public bool isActive { get; set; }
    }
}
