using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.ProposalFormBaseData.Models.View
{
    public class ProposalFormPostPriceMainGrid: GlobalGrid
    {
        public string ppf { get; set; }
        public string title { get; set; }
        public int? price { get; set; }
        public bool? isActive { get; set; }
    }
}
