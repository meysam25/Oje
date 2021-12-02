using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InquiryBaseData.Models.View
{
    public class CreateUpdateInsuranceContractDiscountVM
    {
        public int? id { get; set; }
        public List<int> cIds { get; set; }
        public int? contractId { get; set; }
        public string title { get; set; }
        public int? percent { get; set; }
        public bool? isActive { get; set; }
    }
}
