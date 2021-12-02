using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarThirdBaseData.Models.View
{
    public class ThirdPartyDriverHistoryDamagePenaltyMainGrid: GlobalGrid
    {
        public string title { get; set; }
        public int? percent { get; set; }
        public bool? isActive { get; set; }
    }
}
