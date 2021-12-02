using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InquiryBaseData.Models.View
{
    public class InqueryDescriptionMainGrid: GlobalGrid
    {
        public int? cat { get; set; }
        public string title { get; set; }
        public bool? isActive { get; set; }
        public string ppfTitle { get; set; }
        public int? siteSettingId { get; set; }
    }
}
