using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InquiryBaseData.Models.View
{
    public class CreateUpdateInquiryDurationVM
    {
        public int? id { get; set; }
        public List<int> cIds { get; set; }
        public int? formId { get; set; }
        public string title { get; set; }
        public int? percent { get; set; }
        public int? day { get; set; }
        public bool? isActive { get; set; }
        public string formId_Title { get; internal set; }
    }
}
