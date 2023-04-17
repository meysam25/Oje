using Oje.Infrastructure.Models;

namespace Oje.Section.Tender.Models.View
{
    public class TenderProposalFormJsonConfigFileMainGrid: GlobalGrid
    {
        public string fid { get; set; }
        public string title { get; set; }
        public bool? isActive { get; set; }
    }
}
