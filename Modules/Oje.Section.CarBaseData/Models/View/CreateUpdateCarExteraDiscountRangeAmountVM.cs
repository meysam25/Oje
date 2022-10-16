using System.Collections.Generic;

namespace Oje.Section.CarBaseData.Models.View
{
    public class CreateUpdateCarExteraDiscountRangeAmountVM
    {
        public int? id { get; set; }
        public List<int> cIds { get; set; }
        public int? carExteraDiscountId { get; set; }
        public string carExteraDiscountId_Title { get; set; }
        public int? carExteraDiscountValueId { get; set; }
        public string title { get; set; }
        public long? minValue { get; set; }
        public long? maxValue { get; set; }
        public decimal? percent { get; set; }
        public long? price { get; set; }
        public bool? isActive { get; set; }
        public decimal? cdSelfPercent { get; set; }
    }
}
