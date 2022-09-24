using Oje.Infrastructure.Models;

namespace Oje.Section.CarThirdBaseData.Models.View
{
    public class ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountMainGrid: GlobalGrid
    {
        public string title { get; set; }
        public long? price { get; set; }
        public int? percent { get; set; }
        public int? ctId { get; set; }
        public int? comId { get; set; }
    }
}
