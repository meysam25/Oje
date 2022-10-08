using Oje.Infrastructure.Models;

namespace Oje.Section.InsuranceContractBaseData.Models.View
{
    public class InsuranceContractInsuranceContractTypeMaxPriceCreateUpdateVM: GlobalSiteSetting
    {
        public int? typeId { get; set; }
        public int? cid { get; set; }
        public long? price { get; set; }

        public int? staticTypeId { get; set; }
        public int? staticCid { get; set; }
    }
}
