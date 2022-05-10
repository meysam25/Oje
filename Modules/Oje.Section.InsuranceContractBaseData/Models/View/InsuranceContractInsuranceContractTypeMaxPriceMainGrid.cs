using Oje.Infrastructure.Models;

namespace Oje.Section.InsuranceContractBaseData.Models.View
{
    public class InsuranceContractInsuranceContractTypeMaxPriceMainGrid: GlobalGrid
    {
        public string typeId { get; set; }
        public string cid { get; set; }
        public long? price { get; set; }
    }
}
