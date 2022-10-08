using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using System.Collections.Generic;

namespace Oje.Section.InquiryBaseData.Models.View
{
    public class CreateUpdateInqueryDescriptionVM: GlobalSiteSetting
    {
        public int? id { get; set; }
        public string title { get; set; }
        public int? ppfId { get; set; }
        public string ppfId_Title { get; set; }
        [IgnoreStringEncode]
        public MyHtmlString desc { get; set; }
        public bool? isActive { get; set; }
        public int? settId { get; set; }

        public List<int> cIds { get; set; }

    }
}
