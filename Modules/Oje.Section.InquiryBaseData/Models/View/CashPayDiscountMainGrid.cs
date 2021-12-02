using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InquiryBaseData.Models.View
{
    public class CashPayDiscountMainGrid: GlobalGrid
    {
        public int? company { get; set; }
        public string ppfTitle { get; set; }
        public string title { get; set; }
        public int? percent { get; set; }
        public bool? isActive { get; set; }
    }
}
