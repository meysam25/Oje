using Oje.Infrastructure.Models;

namespace Oje.Section.ProposalFormBaseData.Models.View
{
    public class CreateUpdateProposalFormPostPriceVM: GlobalSiteSetting
    {
        public int? id { get; set; }
        public int? ppfId { get; set; }
        public string ppfId_Title { get; set; }
        public string title { get; set; }
        public int? price { get; set; }
        public bool? isActive { get; set; }
    }
}
