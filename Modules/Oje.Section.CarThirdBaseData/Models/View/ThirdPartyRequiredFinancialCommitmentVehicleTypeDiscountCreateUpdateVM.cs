using Oje.Infrastructure.Models;
using System.Collections.Generic;

namespace Oje.Section.CarThirdBaseData.Models.View
{
    public class ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountCreateUpdateVM: GlobalSiteSetting
    {
        public int? id { get; set; }
        public string title { get; set; }
        public long? price { get; set; }
        public int? percent { get; set; }
        public int? vtId { get; set; }
        public List<int> comIds { get; set; }

    }
}
