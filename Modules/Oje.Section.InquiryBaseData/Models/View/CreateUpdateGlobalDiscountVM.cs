using Oje.Infrastructure.Models;
using System.Collections.Generic;

namespace Oje.Section.InquiryBaseData.Models.View
{
    public class CreateUpdateGlobalDiscountVM: GlobalSiteSetting
    {
        public int? id { get; set; }
        public List<int> cIds { get; set; }
        public int? formId { get; set; }
        public string formId_Title { get; set; }
        public string title { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public string discountCode { get; set; }
        public int? percent { get; set; }
        public long? price { get; set; }
        public long? maxPrice { get; set; }
        public int? countUse { get; set; }
        public string inqueryCode { get; set; }
        public bool? isActive { get; set; }

    }
}
