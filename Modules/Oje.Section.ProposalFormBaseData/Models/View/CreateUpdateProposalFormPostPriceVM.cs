using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.ProposalFormBaseData.Models.View
{
    public class CreateUpdateProposalFormPostPriceVM
    {
        public int? id { get; set; }
        public int? ppfId { get; set; }
        public string title { get; set; }
        public int? price { get; set; }
        public bool? isActive { get; set; }
    }
}
