using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarThirdBaseData.Models.View
{
    public class CreateUpdateThirdPartyDriverFinancialCommitmentVM
    {
        public int? id { get; set; }
        public string title { get; set; }
        public int? year { get; set; }
        public long? price { get; set; }
        public bool? isActive { get; set; }
    }
}
