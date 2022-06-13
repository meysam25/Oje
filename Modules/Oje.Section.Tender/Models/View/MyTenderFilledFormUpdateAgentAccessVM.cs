using System.Collections.Generic;

namespace Oje.Section.Tender.Models.View
{
    public class MyTenderFilledFormUpdateAgentAccessVM
    {
        public long? id { get; set; }
        public int? provinceId { get; set; }
        public int? cityId { get; set; }
        public List<int> cIds { get; set; }
    }
}
