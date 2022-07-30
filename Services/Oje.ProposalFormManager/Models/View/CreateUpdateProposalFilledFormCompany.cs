using System.Collections.Generic;

namespace Oje.ProposalFormService.Models.View
{
    public class CreateUpdateProposalFilledFormCompany
    {
        public long? id { get; set; }
        public List<int> cIds { get; set; }
    }
}
