using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarThirdBaseData.Models.View
{
    public class ThirdPartyRequiredFinancialCommitmentMainGrid: GlobalGrid
    {
        public int? company { get; set; }
        public string title { get; set; }
        public long? price { get; set; }
        public int? order { get; set; }
        public bool? isActive { get; set; }
    }
}
