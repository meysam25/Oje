using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InquiryBaseData.Models.View
{
    public class NoDamageDiscountMainGrid: GlobalGrid
    {
        public string title { get; set; }
        public int? cId { get; set; }
        public string formTitle { get; set; }
        public int? pecent { get; set; }
        public bool? isActive { get; set; }
    }
}
