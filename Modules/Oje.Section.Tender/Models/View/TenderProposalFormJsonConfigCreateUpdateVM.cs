using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;

namespace Oje.Section.Tender.Models.View
{
    public class TenderProposalFormJsonConfigCreateUpdateVM: GlobalSiteSetting
    {
        public int? id { get; set; }
        public int? ppfId { get; set; }
        public string ppfId_Title { get; set; }
        [IgnoreStringEncode]
        public MyHtmlString jsonStr { get; set; }
        [IgnoreStringEncode]
        public MyHtmlString pdfDesc { get; set; }
        public bool? isActive { get; set; }
    }
}
