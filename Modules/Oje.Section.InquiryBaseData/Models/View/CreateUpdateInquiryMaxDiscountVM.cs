using Oje.Infrastructure.Models;
using System.Collections.Generic;

namespace Oje.Section.InquiryBaseData.Models.View
{
    public class CreateUpdateInquiryMaxDiscountVM: GlobalSiteSetting
    {
        public int? id { get; set; }
        public List<int> cIds { get; set; }
        public int? formId { get; set; }
        public string title { get; set; }
        public int? percent { get; set; }
        public bool? isActive { get; set; }
        public string formId_Title { get; internal set; }
    }
}
