using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InquiryBaseData.Models.View
{
    public class GlobalDiscountMainGrid: GlobalGrid
    {
        public int? company { get; set; }
        public string ppfTitle { get; set; }
        public string createUser { get; set; }
        public string createDate { get; set; }
        public string title { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public bool? isActive { get; set; }
    }
}
