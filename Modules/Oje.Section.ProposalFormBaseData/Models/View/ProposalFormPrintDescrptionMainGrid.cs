using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;

namespace Oje.Section.ProposalFormBaseData.Models.View
{
    public class ProposalFormPrintDescrptionMainGrid: GlobalGrid
    {
        public string fTitle { get; set; }
        public ProposalFormPrintDescrptionType? type { get; set; }
        public bool? isActive { get; set; }
    }
}
