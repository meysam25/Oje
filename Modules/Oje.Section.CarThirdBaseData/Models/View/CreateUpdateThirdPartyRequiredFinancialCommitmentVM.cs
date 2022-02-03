using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarThirdBaseData.Models.View
{
    public class CreateUpdateThirdPartyRequiredFinancialCommitmentVM
    {
        public int? id { get; set; }
        public List<int> cIds { get; set; }
        public string title { get; set; }
        public string sTitle { get; set; }
        public long? price { get; set; }
        public int? order { get; set; }
        public bool? isActive { get; set; }
    }
}
