using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.ProposalFormBaseData.Models.View
{

    public class GetByIdProposalFormVM
    {
        public int id { get; set; }
        public string title { get; set; }
        public string name { get; set; }
        [IgnoreStringEncode]
        public MyHtmlString jsonStr { get; set; }
        public string description { get; set; }
        public bool isActive { get; set; }
        public int? siteSettingId { get; set; }
        public int ppfCatId { get; set; }
        public string ppfCatId_Title { get; set; }
        public string rules_address { get; set; }
        public string type { get; set; }
    }
}
