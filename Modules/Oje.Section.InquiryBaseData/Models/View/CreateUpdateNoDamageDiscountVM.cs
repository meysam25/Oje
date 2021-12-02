using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InquiryBaseData.Models.View
{
    public class CreateUpdateNoDamageDiscountVM
    {
        public int? id { get; set; }
        public string title { get; set; }
        public int? ppfId { get; set; }
        public byte? percent { get; set; }
        public int? order { get; set; }
        public bool? isActive { get; set; }
        public List<int> cIds { get; set; }
        public string ppfId_Title { get;  set; }
    }
}
